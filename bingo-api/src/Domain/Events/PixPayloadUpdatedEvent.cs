using bingo_api.src.Entities;
using bingo_api.src.Interfaces;

namespace bingo_api.src.Domain.Events;

public class PixPayloadUpdatedEvent : IDomainEvent
{
     public PaymentMethod PaymentMethod { get; }

    public PixPayloadUpdatedEvent(PaymentMethod paymentMethod)
    {
        PaymentMethod = paymentMethod;
    } 
}
