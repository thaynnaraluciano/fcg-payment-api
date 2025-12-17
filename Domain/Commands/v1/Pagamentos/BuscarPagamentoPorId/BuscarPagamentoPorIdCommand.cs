using MediatR;

namespace Domain.Commands.v1.Pagamentos.BuscarPagamentoPorId
{
    public class BuscarPagamentoPorIdCommand : IRequest<BuscarPagamentoPorIdCommandResponse>
    {
        public Guid Id { get; set; }
    }
}
