using DDD_CQRS.Domain;
using DDD_CQRS.Domain.Repository;

namespace DDD_CQRS.Infrastructure.Repository;

public class ReportRepository : IReportRepository
{
    private static Report _report = new();

    public Report GetReport() => _report;

    public void UpdateReport(Report report) => _report = report;
}
