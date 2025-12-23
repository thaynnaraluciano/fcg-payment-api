using AutoMapper;
using Domain.Enums;
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

        public CriarPagamentoCommandHandler(IPagamentoRepository pagamentoRepository, ILogger<CriarPagamentoCommandHandler> logger, IMapper mapper)
        {
            _pagamentoRepository = pagamentoRepository;
            _logger = logger;
            _mapper = mapper;
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

            _logger.LogInformation("Pagamento criado com Id {IdPagamento}", pagamento.Id);

            return _mapper.Map<CriarPagamentoCommandResponse>(pagamento);
        }
    }
}
