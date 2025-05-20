using MediatR;

namespace DDD_CQRS.Application.Command;

public class CompleteOrder : IRequest
{
    public required Guid OrderId { get; init; }
}
