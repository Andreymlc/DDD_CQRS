using DDD_CQRS.Domain;
using DDD_CQRS.Domain.Events;
using DDD_CQRS.Domain.Repository;
using MediatR;

namespace DDD_CQRS.Application.NotificationHandler;

public class OrderCompletedHandler(IReportRepository reportRepo) : INotificationHandler<OrderCompleted>
{
    public Task Handle(OrderCompleted notification, CancellationToken cancellationToken)
    {
        var currentReport = reportRepo.GetReport();

        var report = new Report
        {
            NumberOrders = currentReport.NumberOrders,
            Income = currentReport.Income,
            NumberCompletedOrders = currentReport.NumberCompletedOrders + 1
        };
        
        reportRepo.UpdateReport(report);
        
        notification.PrintInfo();
        
        return Task.CompletedTask;
    }
}
