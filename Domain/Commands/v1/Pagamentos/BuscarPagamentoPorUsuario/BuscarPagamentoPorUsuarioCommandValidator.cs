using FluentValidation;

namespace Domain.Commands.v1.Pagamentos.BuscarPagamentoPorUsuario
{
    public class BuscarPagamentoPorUsuarioCommandValidator : AbstractValidator<BuscarPagamentoPorUsuarioCommand>
    {
        public BuscarPagamentoPorUsuarioCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("O ID do usuário é obrigatório.");
        }
    }
}
