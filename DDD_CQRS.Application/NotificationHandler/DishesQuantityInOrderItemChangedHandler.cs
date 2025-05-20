
using DDD_CQRS.Application.Helper;
using DDD_CQRS.Domain;
using DDD_CQRS.Domain.Events;
using DDD_CQRS.Domain.Repository;
using MediatR;

namespace DDD_CQRS.Application.NotificationHandler;

public class DishesQuantityInOrderItemChangedHandler(IOrderRepository orderRepo, IReportRepository reportRepo)
    : INotificationHandler<DishesQuantityInOrderItemChanged>
{
    public Task Handle(DishesQuantityInOrderItemChanged notification, CancellationToken cancellationToken)
    {
        var allOrders = orderRepo.FindAll();
        var currentReport = reportRepo.GetReport();
        var mostPopularProduct = OrderHelper
            .UpdateProductSales(notification.Name , notification.Quantity);
        
        var report = new Report
        {
            NumberOrders = allOrders.Count,
            Income = allOrders.Sum(o => o.Cost),
            MostPopularProducts = mostPopularProduct,
            NumberCompletedOrders = currentReport.NumberCompletedOrders
        };
        
        reportRepo.UpdateReport(report);
        
        notification.PrintInfo();
        
        return Task.CompletedTask;
    }
}
