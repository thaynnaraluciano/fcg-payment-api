using Domain.Enums;

namespace Domain.Commands.v1.Pagamentos.ConfirmarPagamento;

public class ConfirmarPagamentoCommandResponse
{
    public Guid Id { get; set; }
    public StatusPagamento Status { get; set; }
}
