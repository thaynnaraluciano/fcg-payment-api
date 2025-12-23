using MediatR;

namespace Domain.Commands.v1.Pagamentos.CriarPagamento
{
    public class CriarPagamentoCommand : IRequest<CriarPagamentoCommandResponse>
    {
        public Guid UserId { get; set; }

        public Guid GameId { get; set; }

        public decimal Valor { get; set; }

        public int MetodoPagamento { get; set; }        
    }
}
