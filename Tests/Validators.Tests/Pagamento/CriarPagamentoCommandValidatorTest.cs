using CommonTestUtilities.Pagamentos;
using Domain.Commands.v1.Pagamentos.CriarPagamento;
using Domain.Enums;
using FluentAssertions;

namespace Validators.Tests.Pagamento
{
    public class CriarPagamentoCommandValidatorTest
    {
        private readonly CriarPagamentoCommandValidator _validator = new();

        [Fact]
        public void ValidarComando_Valido_DeveRetornarSucesso()
        {
            var request = CriarPagamentoCommandBuilder.Build();

            var result = _validator.Validate(request);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Deve_falhar_quando_UserId_esta_vazio()
        {
            var request = CriarPagamentoCommandBuilder.Build();
            request.UserId = Guid.Empty;

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should()
                .Contain(e => e.PropertyName == "UserId" &&
                              e.ErrorMessage == "O usuário é obrigatório.");
        }

        [Fact]
        public void Deve_falhar_quando_GameId_esta_vazio()
        {
            var request = CriarPagamentoCommandBuilder.Build();
            request.GameId = Guid.Empty;

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should()
                .Contain(e => e.PropertyName == "GameId" &&
                              e.ErrorMessage == "O jogo é obrigatório.");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        public void Deve_falhar_quando_valor_e_zero_ou_negativo(decimal valor)
        {
            var request = CriarPagamentoCommandBuilder.Build();
            request.Valor = valor;

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should()
                .Contain(e => e.PropertyName == "Valor" &&
                              e.ErrorMessage == "O valor do pagamento deve ser maior que zero.");
        }

        [Fact]
        public void Deve_falhar_quando_metodo_pagamento_invalido()
        {
            var request = CriarPagamentoCommandBuilder.Build();
            request.MetodoPagamento = 999;

            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should()
                .Contain(e => e.PropertyName == "MetodoPagamento");
        }

        [Theory]
        [InlineData(MetodosPagamento.CartaoDebito)]
        [InlineData(MetodosPagamento.CartaoCredito)]
        [InlineData(MetodosPagamento.Pix)]
        public void Deve_passar_quando_metodo_pagamento_valido(MetodosPagamento metodo)
        {
            var request = CriarPagamentoCommandBuilder.Build();
            request.MetodoPagamento = (int)metodo;

            var result = _validator.Validate(request);

            result.IsValid.Should().BeTrue();
        }
    }
}
