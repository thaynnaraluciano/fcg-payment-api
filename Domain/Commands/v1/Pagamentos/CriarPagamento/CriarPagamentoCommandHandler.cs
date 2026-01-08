using AutoMapper;
using CrossCutting.Exceptions;
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
        private readonly ValidacaoServicosExternos _validacao;

        public CriarPagamentoCommandHandler(IPagamentoRepository pagamentoRepository, ILogger<CriarPagamentoCommandHandler> logger, IMapper mapper, ValidacaoServicosExternos validacao)
        {
            _pagamentoRepository = pagamentoRepository;
            _logger = logger;
            _mapper = mapper;
            _validacao = validacao;
        }

        public async Task<CriarPagamentoCommandResponse> Handle(CriarPagamentoCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Iniciando criação de pagamento. UserId={UserId} GameId={GameId}", request.UserId, request.GameId);

            // 1) valida usuário
            var usuarioExiste = await _validacao.UsuarioExisteAsync(request.UserId);
            if (!usuarioExiste)
            {
                _logger.LogWarning("Usuário não encontrado no UserAPI. UserId={UserId}", request.UserId);
                throw new BusinessException($"Usuário {request.UserId} não encontrado.");
            }

            // 2) valida jogo
            var jogoExiste = await _validacao.JogoExisteAsync(request.GameId);
            if (!jogoExiste)
            {
                _logger.LogWarning("Jogo não encontrado no GameAPI. GameId={GameId}", request.GameId);
                throw new BusinessException($"Jogo {request.GameId} não encontrado.");
            }

            // 3) cria pagamento pendente
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

            _logger.LogInformation("Pagamento criado com sucesso. Id={PagamentoId} Status=Pendente", pagamento.Id);

            return _mapper.Map<CriarPagamentoCommandResponse>(pagamento);
        }
    }
}
