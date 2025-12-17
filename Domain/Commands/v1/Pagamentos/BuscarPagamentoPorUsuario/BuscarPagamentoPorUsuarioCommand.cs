using MediatR;

namespace Domain.Commands.v1.Pagamentos.BuscarPagamentoPorUsuario
{
    public class BuscarPagamentoPorUsuarioCommand : IRequest<IEnumerable<BuscarPagamentoPorUsuarioCommandResponse>>
    {
        public Guid UserId { get; set; }
    }
}
