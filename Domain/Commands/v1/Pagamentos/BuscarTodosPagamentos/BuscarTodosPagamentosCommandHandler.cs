using AutoMapper;
using CrossCutting.Exceptions;
using Infrastructure.Data.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Domain.Commands.v1.Pagamentos.BuscarTodosPagamentos
{
    public class BuscarTodosPagamentosCommandHandler : IRequestHandler<BuscarTodosPagamentosCommand, IEnumerable<BuscarTodosPagamentosCommandResponse>>
    {
        private readonly IPagamentoRepository _pagamentoRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<BuscarTodosPagamentosCommandHandler> _logger;

        public BuscarTodosPagamentosCommandHandler(IPagamentoRepository pagamentoRepository, IMapper mapper, ILogger<BuscarTodosPagamentosCommandHandler> logger)
        {
            _pagamentoRepository = pagamentoRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<BuscarTodosPagamentosCommandResponse>> Handle(BuscarTodosPagamentosCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Buscando todos os pagamentos");

            var pagamentos = await _pagamentoRepository.BuscarTodosPagamentos();

            if (pagamentos == null)
            {
                _logger.LogWarning("Não foi encontrado nenhum pagamento");

                throw new NotFoundException($"Nenhum pagamento foi encontrado.");
            }

            return _mapper.Map<IEnumerable<BuscarTodosPagamentosCommandResponse>>(pagamentos);
        }
    }
}
