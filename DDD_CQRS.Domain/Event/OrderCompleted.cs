using MediatR;

namespace DDD_CQRS.Domain.Events;

public class OrderCompleted : Event.Event, INotification;
