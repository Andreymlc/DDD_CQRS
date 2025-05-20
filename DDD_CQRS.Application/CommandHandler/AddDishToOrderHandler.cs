using DDD_CQRS.Application.Command;
using DDD_CQRS.Domain;
using DDD_CQRS.Domain.Repository;
using MediatR;

namespace DDD_CQRS.Application.CommandHandler;

public class AddDishToOrderHandler(IOrderRepository orderRepo) : IRequestHandler<AddDishToOrder>
{
    public Task Handle(AddDishToOrder command, CancellationToken cancellationToken)
    {
        var order = orderRepo
            .FindById(command.OrderId)
             ?? throw new NullReferenceException("Заказ не найден");
        
        order.AddItem(Dish.Create(Guid.NewGuid(), command.Name, command.Price), command.Quantity);
        orderRepo.AddOrSaveChanges(order);
        
        return Task.CompletedTask;
    }
}
