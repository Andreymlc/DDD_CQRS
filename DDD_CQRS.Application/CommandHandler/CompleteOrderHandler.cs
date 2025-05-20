using DDD_CQRS.Application.Command;
using DDD_CQRS.Domain;
using DDD_CQRS.Domain.Repository;
using MediatR;

namespace DDD_CQRS.Application.CommandHandler;

public class CompleteOrderHandler(IOrderRepository orderRepo) : IRequestHandler<CompleteOrder>
{
    public Task Handle(CompleteOrder command, CancellationToken cancellationToken)
    {
        var order = orderRepo
                        .FindById(command.OrderId)
                         ?? throw new NullReferenceException("Заказ не найден");
        
        order.ChangeStatus(OrderStatus.Completed);
        orderRepo.AddOrSaveChanges(order);
        
        return Task.CompletedTask;
    }
}
