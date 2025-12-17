using FluentValidation;

namespace Domain.Commands.v1.Pagamentos.BuscarTodosPagamentos
{
    public class BuscarTodosPagamentosCommandValidator : AbstractValidator<BuscarTodosPagamentosCommand>
    {
        public BuscarTodosPagamentosCommandValidator()
        {
        }
    }
}
