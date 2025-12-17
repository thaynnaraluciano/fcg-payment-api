namespace Infrastructure.Data.Models
{
    public class PagamentoModel
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid GameId { get; set; }

        public decimal Valor { get; set; }

        public int MetodoPagamento { get; set; }

        public int Status { get; set; }

        public DateTime DataCriacao { get; set; }
    }
}
