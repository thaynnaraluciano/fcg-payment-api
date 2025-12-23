using Infrastructure.Data.Models;

namespace Infrastructure.Data.Interfaces
{
    public interface IPagamentoRepository
    {
        Task CriarPagamentoAsync(PagamentoModel pagamentoModel);
        Task AtualizarPagamentoAsync(PagamentoModel pagamentoModel);
        Task<PagamentoModel> BuscarPagamentoPorId(Guid id);
        Task<IEnumerable<PagamentoModel>> BuscarTodosPagamentos();        
        Task<IEnumerable<PagamentoModel>> BuscarPagamentoPorUsuario(Guid id);
    }
}
