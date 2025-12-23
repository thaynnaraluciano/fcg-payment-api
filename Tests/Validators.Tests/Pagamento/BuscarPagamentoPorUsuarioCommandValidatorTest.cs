using CommonTestUtilities.Pagamentos;
using Domain.Commands.v1.Pagamentos.BuscarPagamentoPorUsuario;
using FluentAssertions;

namespace Validators.Tests.Pagamento
{
    public class BuscarPagamentoPorUsuarioCommandValidatorTest
    {
        private readonly BuscarPagamentoPorUsuarioCommandValidator _validator = new();

        [Fact]
        public void ValidarComando_Valido_DeveRetornarSucesso()
        {
            var command = BuscarPagamentoPorUsuarioCommandBuilder.Build();

            var result = _validator.Validate(command);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Deve_falhar_quando_UserId_esta_vazio()
        {
            var command = BuscarPagamentoPorUsuarioCommandBuilder.Build();
            command.UserId = Guid.Empty;

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should()
                .Contain(e => e.PropertyName == "UserId" &&
                              e.ErrorMessage == "O ID do usuário é obrigatório.");
        }
    }
}
