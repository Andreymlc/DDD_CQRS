using MediatR;

namespace DDD_CQRS.Domain.Events;

public class DishesQuantityInOrderItemChanged : Event.Event, INotification
{
    public required Guid DishId { get; init; }
    public required string Name { get; init; }
    public required int Quantity { get; init; }
}
