using CrossCutting.Messaging.Events;
using MassTransit;

namespace Infrastructure.Messaging.Configurations
{
    public static class MassTransitConfiguration
    {
        public static void Configure(IBusRegistrationContext context, IRabbitMqBusFactoryConfigurator cfg)
        {
            cfg.Host("127.0.0.1", "/", h =>
            {
                h.Username("root");
                h.Password("root");
            });

            // Exchange
            cfg.Message<PaymentConfirmedEvent>(x =>
            {
                x.SetEntityName("payment-confirmed");
            });
        }
    }
}
