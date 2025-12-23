using AutoMapper;
using CrossCutting.Exceptions;
using Infrastructure.Data.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Domain.Commands.v1.Pagamentos.BuscarPagamentoPorId
{
    public class BuscarPagamentoPorIdCommandHandler : IRequestHandler<BuscarPagamentoPorIdCommand, BuscarPagamentoPorIdCommandResponse>
    {
        private readonly IMapper _mapper;
        private readonly IPagamentoRepository _pagamentoRepository;
        private readonly ILogger<BuscarPagamentoPorIdCommandHandler> _logger;

        public BuscarPagamentoPorIdCommandHandler(
            IPagamentoRepository pagamentoRepository,
            IMapper mapper,
            ILogger<BuscarPagamentoPorIdCommandHandler> logger)
        {
            _pagamentoRepository = pagamentoRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BuscarPagamentoPorIdCommandResponse> Handle(BuscarPagamentoPorIdCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Buscando pagamento com ID {PagamentoId}", request.Id);

            var pagamento = await _pagamentoRepository.BuscarPagamentoPorId(request.Id);

            if (pagamento == null)
            {
                _logger.LogWarning("Pagamento com ID {PagamentoId} não encontrado", request.Id);

                throw new NotFoundException($"Pagamento com ID {request.Id} não encontrado.");
            }

            return _mapper.Map<BuscarPagamentoPorIdCommandResponse>(pagamento);
        }
    }
}
