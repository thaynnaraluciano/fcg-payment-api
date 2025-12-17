using FluentValidation;

namespace Domain.Commands.v1.Pagamentos.CriarPagamento
{
    public class CriarPagamentoCommandValidator: AbstractValidator<CriarPagamentoCommand>
    {
        public CriarPagamentoCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("O usuário é obrigatório.");

            RuleFor(x => x.GameId)
                .NotEmpty().WithMessage("O jogo é obrigatório.");

            RuleFor(x => x.Valor)
                .GreaterThan(0)
                .WithMessage("O valor do pagamento deve ser maior que zero.");

            RuleFor(x => x.MetodoPagamento)
                .GreaterThanOrEqualTo(0)
                .LessThan(4)
                .WithMessage("Método de pagamento inválido.");
        }
    }
}
