using Bogus;
using Domain.Commands.v1.Pagamentos.BuscarPagamentoPorUsuario;

namespace CommonTestUtilities.Pagamentos
{
    public class BuscarPagamentoPorUsuarioCommandBuilder
    {
        public static BuscarPagamentoPorUsuarioCommand Build()
        {
            return new Faker<BuscarPagamentoPorUsuarioCommand>()
                .RuleFor(r => r.UserId, faker => faker.Random.Guid());
        }
    }
}
