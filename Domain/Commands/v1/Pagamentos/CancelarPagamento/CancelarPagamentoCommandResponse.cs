using Domain.Enums;

namespace Domain.Commands.v1.Pagamentos.CancelarPagamento
{
    public class CancelarPagamentoCommandResponse
    {
        public Guid Id { get; set; }
        public StatusPagamento Status { get; set; }
    }
}
