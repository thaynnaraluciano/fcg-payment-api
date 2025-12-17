using Domain.Commands.v1.Pagamentos;
using Domain.Commands.v1.Pagamentos.BuscarPagamentoPorId;
using Domain.Commands.v1.Pagamentos.BuscarPagamentoPorUsuario;
using Domain.Commands.v1.Pagamentos.BuscarTodosPagamentos;
using Domain.Commands.v1.Pagamentos.CancelarPagamento;
using Domain.Commands.v1.Pagamentos.CriarPagamento;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/v1/pagamento")]
    public class PagamentosController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PagamentosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST api/v1/pagamentos
        [HttpPost]
        public async Task<IActionResult> CriarPagamento(
            [FromBody] CriarPagamentoCommand command)
        {
            var response = await _mediator.Send(command);
            return Created(string.Empty, response);
        }

        // GET api/v1/pagamentos
        [HttpGet]
        public async Task<IActionResult> BuscarTodosPagamentos()
        {
            var response = await _mediator.Send(new BuscarTodosPagamentosCommand());

            return Ok(response);
        }

        // GET api/v1/pagamentos/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> BuscarPagamentoPorId(Guid id)
        {
            var response = await _mediator.Send(new BuscarPagamentoPorIdCommand { Id = id });

            return Ok(response);
        }

        // GET api/v1/pagamentos/usuario/{userId}
        [HttpGet("usuario/{userId:guid}")]
        public async Task<IActionResult> BuscarPagamentoPorUsuario(Guid userId)
        {
            var response = await _mediator.Send(new BuscarPagamentoPorUsuarioCommand { UserId = userId });

            return Ok(response);
        }

        // PATCH api/v1/pagamentos/{id}/cancelar
        [HttpPatch("{id:guid}/cancelar")]
        public async Task<IActionResult> CancelarPagamento(Guid id)
        {
            var response = await _mediator.Send(new CancelarPagamentoCommand { Id = id });

            return Ok(response);
        }
    }
}
