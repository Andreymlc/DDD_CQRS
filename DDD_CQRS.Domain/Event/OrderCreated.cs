using MediatR;

namespace DDD_CQRS.Domain.Events;

public class OrderCreated : Event.Event, INotification
{
    public required Order Order { get; init; }
}
