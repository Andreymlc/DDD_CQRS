using MediatR;

namespace DDD_CQRS.Domain.Events;

public class DishesQuantityInOrderItemChanged : Event, INotification;
