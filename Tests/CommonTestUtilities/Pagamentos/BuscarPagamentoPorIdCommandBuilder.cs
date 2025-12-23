using Bogus;
using Domain.Commands.v1.Pagamentos.BuscarPagamentoPorId;

namespace CommonTestUtilities.Pagamentos
{
    public class BuscarPagamentoPorIdCommandBuilder
    {
        public static BuscarPagamentoPorIdCommand Build()
        {
            return new Faker<BuscarPagamentoPorIdCommand>()
                .RuleFor(r => r.Id, faker => faker.Random.Guid());
        }
    }
}
