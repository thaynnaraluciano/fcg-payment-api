using FluentValidation;

namespace Domain.Commands.v1.Pagamentos.BuscarPagamentoPorId
{
    public class BuscarPagamentoPorIdCommandValidator : AbstractValidator<BuscarPagamentoPorIdCommand>
    {
        public BuscarPagamentoPorIdCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("O Id do pagamento é obrigatório.")
                .NotNull().WithMessage("O Id do pagamento não pode ser nulo.");
        }
    }
}
