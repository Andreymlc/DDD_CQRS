using DDD_CQRS.Domain;
using DDD_CQRS.Domain.Events;
using DDD_CQRS.Domain.Repository;
using MediatR;

namespace DDD_CQRS.Application.NotificationHandler;

public class DishAddedToOrderHandler(IOrderRepository orderRepo, IReportRepository reportRepo)
    : INotificationHandler<DishAddedToOrderEvent>
{
    public Task Handle(DishAddedToOrderEvent notification, CancellationToken cancellationToken)
    {
        var allOrders = orderRepo.FindAll();
        var currentReport = reportRepo.GetReport();
        
        var report = new Report
        {
            NumberOrders = allOrders.Count,
            Income = allOrders.Sum(o => o.Cost),
            NumberCompletedOrders = currentReport.NumberCompletedOrders
        };
        
        reportRepo.UpdateReport(report);
        
        return Task.CompletedTask;
    }
}
