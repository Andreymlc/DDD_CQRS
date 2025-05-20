using DDD_CQRS.Application.Query;
using DDD_CQRS.Domain;
using DDD_CQRS.Domain.Repository;
using MediatR;

namespace DDD_CQRS.Application.QueryHandler;

public class GetOrderHistoryHandler(IOrderRepository orderRepo) : IRequestHandler<GetOrderHistory, IEnumerable<Order>>
{
    public Task<IEnumerable<Order>> Handle(GetOrderHistory query, CancellationToken cancellationToken) => 
        Task.FromResult(orderRepo.FindAll().Take(query.Number));
}
