using FluentValidation;

namespace TesteCamposDealer.Application.Handlers.Clientes.Commands.UpdateCliente
{
    public class UpdateClienteCommandValidator : AbstractValidator<UpdateClienteCommand>
    {
        public UpdateClienteCommandValidator()
        {
            RuleFor(x => x.nomeCliente)
                .NotEmpty().WithMessage("Nome do cliente é obrigatório.")
                .MaximumLength(200).WithMessage("Nome do cliente deve ter no máximo 200 caracteres.");

            RuleFor(x => x.endereco)
                .NotEmpty().WithMessage("Endereço é obrigatório.")
                .MaximumLength(30).WithMessage("Endereço deve ter no máximo 30 caracteres.");
        }
    }
}
