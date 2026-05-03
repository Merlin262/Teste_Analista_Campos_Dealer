using FluentValidation;

namespace TesteCamposDealer.Application.Handlers.Produtos.Commands
{
    public class CreateProdutoCommandValidator : AbstractValidator<CreateProdutoCommand>
    {
        public CreateProdutoCommandValidator()
        {
            RuleFor(x => x.dscProduto)
                .NotEmpty().WithMessage("Descrição do produto é obrigatória.")
                .MaximumLength(100).WithMessage("Descrição do produto deve ter no máximo 100 caracteres.");

            RuleFor(x => x.vlrProduto)
                .GreaterThan(0).WithMessage("Valor do produto deve ser maior que zero.");
        }
    }
}
