using AutoMapper;
using CrossCutting.Exceptions;
using Domain.Enums;
using Domain.Interfaces;
using Infrastructure.Data.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Domain.Commands.v1.Pagamentos.CancelarPagamento
{
    public class CancelarPagamentoCommandHandler : IRequestHandler<CancelarPagamentoCommand, CancelarPagamentoCommandResponse>
    {
        private readonly IPagamentoRepository _pagamentoRepository;
        private readonly IPagamentoNotificacaoService _notificacaoService;
        private readonly IMapper _mapper;
        private readonly ILogger<CancelarPagamentoCommandHandler> _logger;

        public CancelarPagamentoCommandHandler(
            IPagamentoRepository pagamentoRepository,
            IMapper mapper,
            ILogger<CancelarPagamentoCommandHandler> logger,
            IPagamentoNotificacaoService notificacaoService)
        {
            _pagamentoRepository = pagamentoRepository;
            _mapper = mapper;
            _logger = logger;
            _notificacaoService = notificacaoService;
        }

        public async Task<CancelarPagamentoCommandResponse> Handle(CancelarPagamentoCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Solicitando cancelamento do pagamento {PagamentoId}", request.Id);

            var pagamento = await _pagamentoRepository.BuscarPagamentoPorId(request.Id);

            if (pagamento == null)
            {
                _logger.LogWarning("Pagamento {PagamentoId} não encontrado para cancelamento", request.Id);

                throw new NotFoundException($"Pagamento com ID {request.Id} não encontrado.");
            }

            if (pagamento.Status != (int)StatusPagamento.Pendente)
            {
                _logger.LogWarning("Pagamento {PagamentoId} não pode ser cancelado. Status atual: {Status}", request.Id, pagamento.Status);

                throw new BusinessException("Somente pagamentos pendentes podem ser cancelados.");
            }

            pagamento.Status = (int)StatusPagamento.Cancelado;

            await _pagamentoRepository.AtualizarPagamentoAsync(pagamento);

            _logger.LogInformation("Pagamento {PagamentoId} cancelado com sucesso", request.Id);

            // Dispara notificação APÓS salvar o status cancelado
            await _notificacaoService.NotificarAsync(
                pagamento.Id,
                pagamento.UserId,
                pagamento.Valor,
                (StatusPagamento)pagamento.Status
            );

            return _mapper.Map<CancelarPagamentoCommandResponse>(pagamento);
        }
    }
}
