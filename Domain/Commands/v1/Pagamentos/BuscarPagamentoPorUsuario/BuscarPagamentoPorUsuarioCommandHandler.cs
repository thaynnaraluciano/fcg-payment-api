using AutoMapper;
using CrossCutting.Exceptions;
using Infrastructure.Data.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Domain.Commands.v1.Pagamentos.BuscarPagamentoPorUsuario
{
    public class BuscarPagamentoPorUsuarioCommandHandler : IRequestHandler<BuscarPagamentoPorUsuarioCommand, IEnumerable<BuscarPagamentoPorUsuarioCommandResponse>>
    {
        private readonly IPagamentoRepository _pagamentoRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<BuscarPagamentoPorUsuarioCommandHandler> _logger;

        public BuscarPagamentoPorUsuarioCommandHandler(
            IPagamentoRepository pagamentoRepository,
            IMapper mapper,
            ILogger<BuscarPagamentoPorUsuarioCommandHandler> logger)
        {
            _pagamentoRepository = pagamentoRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<BuscarPagamentoPorUsuarioCommandResponse>> Handle(BuscarPagamentoPorUsuarioCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Buscando pagamentos do usuário {UserId}", request.UserId);

            var pagamentos = await _pagamentoRepository.BuscarPagamentoPorUsuario(request.UserId);

            if (pagamentos == null)
            {
                _logger.LogWarning("UserId {UserId} não encontrado", request.UserId);

                throw new NotFoundException($"Usuário {request.UserId} não encontrado.");
            }

            if (!pagamentos.Any())
            {
                _logger.LogWarning("Pagamento com UserId {PagamentoId} não encontrado", request.UserId);

                throw new NotFoundException($"Não existe pagamentos para o usuário {request.UserId}.");
            }

            return _mapper.Map<IEnumerable<BuscarPagamentoPorUsuarioCommandResponse>>(pagamentos);
        }
    }
}
