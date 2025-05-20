using DDD_CQRS.Application.Query;
using DDD_CQRS.Domain;
using DDD_CQRS.Domain.Repository;
using MediatR;

namespace DDD_CQRS.Application.QueryHandler;

public class GetReportHandler(IReportRepository reportRepo) 
    : IRequestHandler<GetReport, Report>
{
    public Task<Report> Handle(GetReport query, CancellationToken cancellationToken) =>
        Task.FromResult(reportRepo.GetReport());
}
