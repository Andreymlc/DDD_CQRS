using DDD_CQRS.Domain;
using MediatR;

namespace DDD_CQRS.Application.Query;

public class GetOrderHistory : IRequest<IReadOnlyList<Order>>
{
    public required int NumberDays { get; init; }
}
