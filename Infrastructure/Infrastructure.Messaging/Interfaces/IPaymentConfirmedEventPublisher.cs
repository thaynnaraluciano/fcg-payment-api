using CrossCutting.Messaging.Events;

namespace Infrastructure.Messaging.Interfaces
{
    public interface IPaymentConfirmedEventPublisher
    {
        Task PublishPaymentConfirmed(PaymentConfirmedEvent paymentConfirmedEvent);
    }
}
