using DDD_CQRS.Application.Query;
using DDD_CQRS.Domain;
using DDD_CQRS.Domain.Repository;
using MediatR;

namespace DDD_CQRS.Application.QueryHandler;

public class GetAllOrdersHandler(IOrderRepository orderRepo) : IRequestHandler<GetAllOrders, IReadOnlyList<Order>>
{
    public Task<IReadOnlyList<Order>> Handle(GetAllOrders request, CancellationToken cancellationToken) => 
        Task.FromResult(orderRepo.FindAll());
}
