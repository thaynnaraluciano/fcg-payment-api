using Bogus;
using Domain.Commands.v1.Pagamentos.ConfirmarPagamento;

namespace CommonTestUtilities.Pagamentos;

public class ConfirmarPagamentoCommandBuilder
{
    public static ConfirmarPagamentoCommand Build()
    {
        return new Faker<ConfirmarPagamentoCommand>()
            .RuleFor(r => r.Id, faker => faker.Random.Guid());
    }
}
