using MediatR;

namespace DDD_CQRS.Application.Command;

public class ChangeDishesQuantityInOrderItem : IRequest
{
    public required Guid OrderId { get; init; }
    public required Guid DishId { get; init; }
    public required int NewQuantity { get; init; }
}
