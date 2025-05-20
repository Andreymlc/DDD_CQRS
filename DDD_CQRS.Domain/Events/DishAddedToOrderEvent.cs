using MediatR;

namespace DDD_CQRS.Domain.Events;

public class DishAddedToOrderEvent : Event, INotification;
