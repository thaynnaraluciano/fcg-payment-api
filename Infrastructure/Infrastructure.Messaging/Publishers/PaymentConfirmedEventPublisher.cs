using CrossCutting.Messaging.Events;
using Infrastructure.Messaging.Interfaces;
using MassTransit;

namespace Infrastructure.Messaging.Publishers
{
    public class PaymentConfirmedEventPublisher : IPaymentConfirmedEventPublisher
    {
        private readonly IPublishEndpoint _publish;

        public PaymentConfirmedEventPublisher(IPublishEndpoint publish)
        {
            _publish = publish;
        }

        public Task PublishPaymentConfirmed(PaymentConfirmedEvent paymentConfirmedEvent) 
            => _publish.Publish(paymentConfirmedEvent);
    }
}
