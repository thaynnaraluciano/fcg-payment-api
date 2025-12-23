using Bogus;
using Domain.Commands.v1.Pagamentos.CancelarPagamento;

namespace CommonTestUtilities.Pagamentos
{
    public class CancelarPagamentoCommandBuilder
    {
        public static CancelarPagamentoCommand Build()
        {
            return new Faker<CancelarPagamentoCommand>()
                .RuleFor(r => r.Id, faker => faker.Random.Guid());
        }
    }
}
