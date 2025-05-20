using MediatR;

namespace DDD_CQRS.Application.Command;

public class AddDishToOrder : IRequest
{
    public required Guid OrderId { get; init; }
    public required string Name { get; init; }
    public required decimal Price { get; init; }
    public required int Quantity { get; init; }
}
