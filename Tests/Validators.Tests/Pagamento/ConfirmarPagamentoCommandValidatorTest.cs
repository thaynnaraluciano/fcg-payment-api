using CommonTestUtilities.Pagamentos;
using Domain.Commands.v1.Pagamentos.ConfirmarPagamento;
using FluentAssertions;

namespace Validators.Tests.Pagamento;

public class ConfirmarPagamentoCommandValidatorTest
{
    private readonly ConfirmarPagamentoCommandValidator _validator = new();

    [Fact]
    public void ValidarComando_Valido_DeveRetornarSucesso()
    {
        var command = ConfirmarPagamentoCommandBuilder.Build();
        var result = _validator.Validate(command);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Deve_falhar_quando_Id_esta_vazio()
    {
        var command = ConfirmarPagamentoCommandBuilder.Build();
        command.Id = Guid.Empty;
        var result = _validator.Validate(command);
        result.IsValid.Should().BeFalse();
        result.Errors.Should()
            .Contain(e => e.PropertyName == "Id" &&
                          e.ErrorMessage == "O ID do pagamento é obrigatório.");
    }
}
