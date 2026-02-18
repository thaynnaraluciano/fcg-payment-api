using CrossCutting.Messaging.Events;
using MassTransit;

namespace Infrastructure.Messaging.Configurations
{
    public static class MassTransitConfiguration
    {
        public static void Configure(IBusRegistrationContext context, IRabbitMqBusFactoryConfigurator cfg)
        {
            var rabbitUrl = Environment.GetEnvironmentVariable("RABBITMQ_URL") ?? "rabbitmq://127.0.0.1/";

            cfg.Host(new Uri(rabbitUrl));

            // Exchange
            cfg.Message<PaymentConfirmedEvent>(x =>
            {
                x.SetEntityName("payment-confirmed");
            });
        }
    }
}
