using MediatR;

namespace Domain.Commands.v1.Pagamentos.CancelarPagamento
{
    public class CancelarPagamentoCommand : IRequest<CancelarPagamentoCommandResponse>
    {
        public Guid Id { get; set; }
    }
}
