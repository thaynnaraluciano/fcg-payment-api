using Domain.Enums;

namespace Domain.Commands.v1.Pagamentos.BuscarTodosPagamentos
{
    public class BuscarTodosPagamentosCommandResponse
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid GameId { get; set; }
        public decimal Valor { get; set; }
        public MetodosPagamento MetodoPagamento { get; set; }
        public StatusPagamento Status { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}
