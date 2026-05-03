using FluentValidation;

namespace TesteCamposDealer.Application.Handlers.Vendas.Commands.UpdateVenda
{
    public class UpdateVendaCommandValidator : AbstractValidator<UpdateVendaCommand>
    {
        public UpdateVendaCommandValidator()
        {
            RuleFor(x => x.itens)
                .NotEmpty().WithMessage("A venda deve conter ao menos um item.");

            RuleForEach(x => x.itens).ChildRules(item =>
            {
                item.RuleFor(i => i.idProduto)
                    .NotEmpty().WithMessage("Produto do item é obrigatório.");

                item.RuleFor(i => i.quantidade)
                    .GreaterThan(0).WithMessage("Quantidade do item deve ser maior que zero.");
            });
        }
    }
}
