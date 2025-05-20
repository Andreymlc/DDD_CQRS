using DDD_CQRS.Domain;
using MediatR;

namespace DDD_CQRS.Application.Query;

public class GetAllOrders : IRequest<IReadOnlyList<Order>>;
