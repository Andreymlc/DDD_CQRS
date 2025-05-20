using DDD_CQRS.Domain;
using MediatR;

namespace DDD_CQRS.Application.Command;

public class ChangeOrderStatus : IRequest
{
    public required Guid OrderId { get; init; }
    public required OrderStatus OrderStatus { get; init; }
}
