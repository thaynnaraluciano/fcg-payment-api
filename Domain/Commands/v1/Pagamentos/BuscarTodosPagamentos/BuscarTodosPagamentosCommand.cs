using MediatR;

namespace Domain.Commands.v1.Pagamentos.BuscarTodosPagamentos
{
    public class BuscarTodosPagamentosCommand : IRequest<IEnumerable<BuscarTodosPagamentosCommandResponse>>
    {
    }
}
