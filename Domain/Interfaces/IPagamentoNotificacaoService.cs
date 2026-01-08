using Domain.Enums;

namespace Domain.Interfaces;

public interface IPagamentoNotificacaoService
{
    Task NotificarAsync(Guid pagamentoId, Guid UserId, decimal Valor, StatusPagamento status);
}
