using MediatR;

namespace Domain.Commands.v1.Pagamentos.ConfirmarPagamento;

public class ConfirmarPagamentoCommand : IRequest<ConfirmarPagamentoCommandResponse>
{
    public Guid Id { get; set; }
}
