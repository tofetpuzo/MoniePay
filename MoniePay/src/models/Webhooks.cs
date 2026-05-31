namespace MoniePay.src.models
{
    public class Webhooks
    {
        public Guid Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public string EventType { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        public WebhookDeliveries? WebhookDeliveries { get; set; }

        public Webhooks() { }

        public Webhooks(Guid id, string url, string eventType, bool isActive, DateTime createdAt)
        {
            Id = id;
            Url = url;
            EventType = eventType;
            IsActive = isActive;
            CreatedAt = createdAt;
        }
    }
}
