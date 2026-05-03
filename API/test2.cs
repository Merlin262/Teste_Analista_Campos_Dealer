using System;
using System.Reflection;
using System.Linq;
using MediatR;
using TesteCamposDealer.Models;
using TesteCamposDealer.Application.Handlers.Clientes.Queries.GetAllClientes;
using TesteCamposDealer.DB;
using TesteCamposDealer.Domain.Interface;
using TesteCamposDealer.Infrastructure.Repositories;

class Program {
    static void Main() {
        try {
            var appAssembly = typeof(GetAllClientesHandler).Assembly;
            var mediator = BuildMediator(appAssembly);
            var result = mediator.Send(new GetAllClientesQuery(1)).GetAwaiter().GetResult();
            Console.WriteLine(""Success!"");
        } catch(Exception e) {
            Console.WriteLine(""EXCEPTION: "" + e.ToString());
        }
    }

    private static IMediator BuildMediator(Assembly appAssembly)
    {
        return new Mediator(serviceType =>
        {
            if (serviceType.IsGenericType && serviceType.GetGenericTypeDefinition() == typeof(System.Collections.Generic.IEnumerable<>))
            {
                var itemType = serviceType.GetGenericArguments()[0];
                if (itemType.IsGenericType && itemType.GetGenericTypeDefinition() == typeof(IPipelineBehavior<,>))
                {
                    var reqType = itemType.GetGenericArguments()[0];
                    var resType = itemType.GetGenericArguments()[1];
                    
                    var valBehaviorType = appAssembly.GetType(""TesteCamposDealer.Application.Behaviors.ValidationBehavior"" + ""2"").MakeGenericType(reqType, resType);
                    var validatorType = typeof(FluentValidation.IValidator<>).MakeGenericType(reqType);
                    
                    var validatorImplTypes = appAssembly.GetTypes()
                        .Where(t => !t.IsAbstract && !t.IsInterface && validatorType.IsAssignableFrom(t)).ToArray();

                    var validatorsArray = Array.CreateInstance(validatorType, validatorImplTypes.Length);
                    for (int i = 0; i < validatorImplTypes.Length; i++)
                    {
                        validatorsArray.SetValue(Activator.CreateInstance(validatorImplTypes[i]), i);
                    }

                    var validationBehavior = Activator.CreateInstance(valBehaviorType, validatorsArray);
                    var behaviorArray = Array.CreateInstance(itemType, 1);
                    behaviorArray.SetValue(validationBehavior, 0);

                    return behaviorArray;
                }

                return Array.CreateInstance(itemType, 0);
            }

            var handlerType = appAssembly.GetTypes()
                .FirstOrDefault(t => !t.IsAbstract && !t.IsInterface && serviceType.IsAssignableFrom(t));

            if (handlerType == null) return null;

            return Activator.CreateInstance(handlerType, new UnitOfWork(new AppDbContext()));
        });
    }
}
