namespace DDD_CQRS.Domain.Repository;

public interface IReportRepository
{
    Report GetReport();
    void UpdateReport(Report report);
}
