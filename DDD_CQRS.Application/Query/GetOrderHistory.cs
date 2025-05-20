using DDD_CQRS.Domain;
using MediatR;

namespace DDD_CQRS.Application.Query;

public class GetOrderHistory : IRequest<IEnumerable<Order>>
{
    public required int Number { get; init; }
}
