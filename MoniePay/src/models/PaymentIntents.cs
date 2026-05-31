namespace MoniePay.src.models
{
    public class PaymentIntents
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Channel { get; set; } = string.Empty;
        public string IdempotencyKey { get; set; } = string.Empty;
        public string Reference { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Customer? Customer { get; set; }
        public Payments? Payment { get; set; }
        public Transactions? Transaction { get; set; }
        public Payouts? Payout { get; set; }
        public PaymentAttempts? PaymentAttempts { get; set; }

        public PaymentIntents() { }

        public PaymentIntents(Guid id, Guid customerId, decimal amount, string currency, string status, string channel, string idempotencyKey, string reference, DateTime createdAt, DateTime updatedAt)
        {
            Id = id;
            CustomerId = customerId;
            Amount = amount;
            Currency = currency;
            Status = status;
            Channel = channel;
            IdempotencyKey = idempotencyKey;
            Reference = reference;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }
    }
}
