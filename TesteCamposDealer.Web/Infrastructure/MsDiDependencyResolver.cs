using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TesteCamposDealer.Web.Infrastructure
{
    public class MsDiDependencyResolver : IDependencyResolver
    {
        private readonly IServiceProvider _provider;

        public MsDiDependencyResolver(IServiceProvider provider) { _provider = provider; }

        public object GetService(Type serviceType) => _provider.GetService(serviceType);
        public IEnumerable<object> GetServices(Type serviceType)
        {
            var enumerableType = typeof(IEnumerable<>).MakeGenericType(serviceType);
            var services = _provider.GetService(enumerableType);
            return services != null ? (IEnumerable<object>)services : Enumerable.Empty<object>();
        }
    }
}