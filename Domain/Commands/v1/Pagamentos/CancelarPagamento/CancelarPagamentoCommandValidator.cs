using FluentValidation;

namespace Domain.Commands.v1.Pagamentos.CancelarPagamento
{
    public class CancelarPagamentoCommandValidator : AbstractValidator<CancelarPagamentoCommand>
    {
        public CancelarPagamentoCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("O ID do pagamento é obrigatório.");
        }
    }
}
