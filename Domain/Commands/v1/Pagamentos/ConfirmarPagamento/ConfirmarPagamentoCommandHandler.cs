using AutoMapper;
using CrossCutting.Exceptions;
using Domain.Enums;
using Domain.Interfaces;
using Infrastructure.Data.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Domain.Commands.v1.Pagamentos.ConfirmarPagamento;

public class ConfirmarPagamentoCommandHandler : IRequestHandler<ConfirmarPagamentoCommand, ConfirmarPagamentoCommandResponse>
{
    private readonly IPagamentoRepository _pagamentoRepository;
    private readonly IPagamentoNotificacaoService _notificacaoService;
    private readonly IMapper _mapper;
    private readonly ILogger<ConfirmarPagamentoCommandHandler> _logger;

    public ConfirmarPagamentoCommandHandler(
        IPagamentoRepository pagamentoRepository,
        IPagamentoNotificacaoService notificacaoService,
        IMapper mapper,
        ILogger<ConfirmarPagamentoCommandHandler> logger)
    {
        _pagamentoRepository = pagamentoRepository;
        _notificacaoService = notificacaoService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ConfirmarPagamentoCommandResponse> Handle(ConfirmarPagamentoCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Solicitando confirmação do pagamento {PagamentoId}", request.Id);

        var pagamento = await _pagamentoRepository.BuscarPagamentoPorId(request.Id);

        if (pagamento == null)
        {
            _logger.LogWarning("Pagamento {PagamentoId} não encontrado para confirmação", request.Id);
            throw new NotFoundException($"Pagamento com ID {request.Id} não encontrado.");
        }

        // Regra: só confirma se estiver pendente
        if (pagamento.Status != (int)StatusPagamento.Pendente)
        {
            _logger.LogWarning("Pagamento {PagamentoId} não pode ser confirmado. Status atual: {Status}", request.Id, pagamento.Status);
            throw new BusinessException("Somente pagamentos pendentes podem ser confirmados.");
        }

        pagamento.Status = (int)StatusPagamento.Aprovado;

        await _pagamentoRepository.AtualizarPagamentoAsync(pagamento);

        _logger.LogInformation("Pagamento {PagamentoId} confirmado com sucesso", request.Id);

        // Dispara notificação APÓS salvar o status pago
        await _notificacaoService.NotificarAsync(
            pagamento.Id,
            pagamento.UserId,
            pagamento.Valor,
            (StatusPagamento)pagamento.Status
        );

        return _mapper.Map<ConfirmarPagamentoCommandResponse>(pagamento);
    }
}