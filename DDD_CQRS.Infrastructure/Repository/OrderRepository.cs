using DDD_CQRS.Domain;
using DDD_CQRS.Domain.Repository;
using MediatR;

namespace DDD_CQRS.Infrastructure.Repository;

public class OrderRepository(IMediator mediator) : IOrderRepository
{
    private static readonly Dictionary<Guid, Order> _orders = new();

    public Order? FindById(Guid id) => _orders[id];

    public IReadOnlyList<Order> FindAll() => _orders.Values.ToList();
    
    public void AddOrSaveChanges(Order order)
    {
        _orders.TryAdd(order.Id, order);
        
        foreach (var domainEvent in order.DomainEvents)
            mediator.Publish(domainEvent);
    }
}
