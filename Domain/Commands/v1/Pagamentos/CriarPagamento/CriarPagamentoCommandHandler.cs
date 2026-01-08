using AutoMapper;
using Domain.Enums;
using Domain.Interfaces;
using Infrastructure.Data.Interfaces;
using Infrastructure.Data.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Domain.Commands.v1.Pagamentos.CriarPagamento
{
    public class CriarPagamentoCommandHandler : IRequestHandler<CriarPagamentoCommand, CriarPagamentoCommandResponse>
    {
        private readonly IPagamentoRepository _pagamentoRepository;
        private readonly ILogger<CriarPagamentoCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IPagamentoNotificacaoService _notification;

        public CriarPagamentoCommandHandler(IPagamentoRepository pagamentoRepository, ILogger<CriarPagamentoCommandHandler> logger, IMapper mapper, IPagamentoNotificacaoService notification)
        {
            _pagamentoRepository = pagamentoRepository;
            _logger = logger;
            _mapper = mapper;
            _notification = notification;
        }

        public async Task<CriarPagamentoCommandResponse> Handle(CriarPagamentoCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Criando pagamento para o usuário {UserId} e jogo {GameId}", request.UserId, request.GameId);

            var pagamento = new PagamentoModel
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                GameId = request.GameId,
                Valor = request.Valor,
                MetodoPagamento = request.MetodoPagamento,
                Status = (int)StatusPagamento.Pendente,
                DataCriacao = DateTime.UtcNow
            };

            await _pagamentoRepository.CriarPagamentoAsync(pagamento);

            await _notification.NotificarAsync(
                pagamento.Id,
                pagamento.UserId,
                pagamento.Valor,
                (StatusPagamento)pagamento.Status
            );

            _logger.LogInformation("Pagamento criado com Id {IdPagamento}", pagamento.Id);

            return _mapper.Map<CriarPagamentoCommandResponse>(pagamento);
        }
    }
}
