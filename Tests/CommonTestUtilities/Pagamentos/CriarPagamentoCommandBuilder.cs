using Bogus;
using Domain.Commands.v1.Pagamentos.CriarPagamento;
using Domain.Enums;

namespace CommonTestUtilities.Pagamentos
{
    public class CriarPagamentoCommandBuilder
    {
        public static CriarPagamentoCommand Build()
        {
            return new Faker<CriarPagamentoCommand>()
                .RuleFor(r => r.UserId, faker => faker.Random.Guid())
                .RuleFor(r => r.GameId, faker => faker.Random.Guid())
                .RuleFor(r => r.Valor, faker => faker.Finance.Amount(10, 500))
                .RuleFor(r => r.MetodoPagamento, faker => (int)faker.PickRandom<MetodosPagamento>());
        }
    }
}
