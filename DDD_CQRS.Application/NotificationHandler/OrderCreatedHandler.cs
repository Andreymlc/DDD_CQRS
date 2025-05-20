using DDD_CQRS.Application.Helper;
using DDD_CQRS.Domain;
using DDD_CQRS.Domain.Events;
using DDD_CQRS.Domain.Repository;
using MediatR;

namespace DDD_CQRS.Application.NotificationHandler;

public class OrderCreatedHandler(IOrderRepository orderRepo, IReportRepository reportRepo)
    : INotificationHandler<OrderCreated>
{
    public Task Handle(OrderCreated notification, CancellationToken cancellationToken)
    {
        var currentReport = reportRepo.GetReport();
        var allOrders = orderRepo.FindAll();
        var mostPopularProduct = OrderHelper.UpdateProductSales(notification.Order);

        var report = new Report
        {
            NumberOrders = currentReport.NumberOrders + 1,
            Income = allOrders.Sum(o => o.Cost),
            MostPopularProducts = mostPopularProduct,
            NumberCompletedOrders = currentReport.NumberCompletedOrders
        };
        
        reportRepo.UpdateReport(report);
        
        notification.PrintInfo();
        
        return Task.CompletedTask;
    }
}
