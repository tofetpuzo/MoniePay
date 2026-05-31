namespace MoniePay.src.models
{
    public class Transactions
    {
        public Guid Id { get; set; }
        public Guid PaymentIntentId { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        public LedgerEntries? LedgerEntries { get; set; }

        public Transactions() { }

        public Transactions(Guid id, Guid paymentIntentId, string status, DateTime createdAt)
        {
            Id = id;
            PaymentIntentId = paymentIntentId;
            Status = status;
            CreatedAt = createdAt;
        }
    }
}
