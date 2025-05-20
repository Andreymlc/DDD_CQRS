namespace DDD_CQRS.Domain.Repository;

public interface IOrderRepository
{
    Order? FindById(Guid id);

    IReadOnlyList<Order> FindAll();
    void AddOrSaveChanges(Order order);
}
