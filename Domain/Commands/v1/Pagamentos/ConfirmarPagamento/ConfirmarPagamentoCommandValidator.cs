using FluentValidation;

namespace Domain.Commands.v1.Pagamentos.ConfirmarPagamento
{
    public class ConfirmarPagamentoCommandValidator : AbstractValidator<ConfirmarPagamentoCommand>
    {
        public ConfirmarPagamentoCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("O ID do pagamento é obrigatório.");
        }
    }
}
