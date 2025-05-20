using DDD_CQRS.Domain;
using MediatR;

namespace DDD_CQRS.Application.Query;

public class GetReport : IRequest<Report>;
