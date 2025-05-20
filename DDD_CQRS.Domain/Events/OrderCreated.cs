using MediatR;

namespace DDD_CQRS.Domain.Events;

public class OrderCreated : Event, INotification;
