namespace DDD_CQRS.Domain.Events;

public abstract class Event
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTimeOffset TimeStamp { get; } = DateTimeOffset.Now;
    public string? Description { get; set; }
}
