using OnlineTravel.Domain.Entities._Base;

namespace OnlineTravel.Domain.Entities.Payments;

public class ProcessedWebhookEvent : BaseEntity
{
    public string EventId { get; private set; } = null!;
    public string Provider { get; private set; } = null!;
    public DateTime ProcessedAt { get; private set; } = DateTime.UtcNow;

    protected ProcessedWebhookEvent() { }

    public static ProcessedWebhookEvent Create(string eventId, string provider)
    {
        return new ProcessedWebhookEvent
        {
            Id = Guid.NewGuid(),
            EventId = eventId,
            Provider = provider,
            ProcessedAt = DateTime.UtcNow
        };
    }
}
