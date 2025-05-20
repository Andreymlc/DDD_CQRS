using DDD_CQRS.Application.Query;
using DDD_CQRS.Domain;
using DDD_CQRS.Domain.Repository;
using MediatR;

namespace DDD_CQRS.Application.QueryHandler;

public class GetOrderHistoryHandler(IOrderRepository orderRepo) : IRequestHandler<GetOrderHistory, IReadOnlyList<Order>>
{
    public Task<IReadOnlyList<Order>> Handle(GetOrderHistory query, CancellationToken cancellationToken) => 
        Task.FromResult((IReadOnlyList<Order>)orderRepo.FindAll().Take(query.NumberDays));
}
