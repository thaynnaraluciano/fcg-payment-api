using Infrastructure.Data.Context;
using Infrastructure.Data.Interfaces;
using Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class PagamentoRepository : IPagamentoRepository
    {
        private readonly AppDbContext _context;

        public PagamentoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AtualizarPagamentoAsync(PagamentoModel pagamentoModel)
        {
            _context.Pagamentos.Update(pagamentoModel);
            await _context.SaveChangesAsync();
        }

        public async Task<PagamentoModel> BuscarPagamentoPorId(Guid id)
        {
            return await _context.Pagamentos.AsNoTracking().FirstAsync(pagamento => pagamento.Id == id);
        }

        public async Task<IEnumerable<PagamentoModel>> BuscarPagamentoPorUsuario(Guid id)
        {
            return await _context.Pagamentos.AsNoTracking().Where(pagamento => pagamento.UserId == id).OrderByDescending(p => p.DataCriacao).ToListAsync();
        }

        public async Task<IEnumerable<PagamentoModel>> BuscarTodosPagamentos()
        {
            return await _context.Pagamentos.AsNoTracking().OrderByDescending(pagamento => pagamento.DataCriacao).ToListAsync();
        }

        public async Task CriarPagamentoAsync(PagamentoModel pagamentoModel)
        {
            await _context.Pagamentos.AddAsync(pagamentoModel);
            await _context.SaveChangesAsync();
        }
    }
}
