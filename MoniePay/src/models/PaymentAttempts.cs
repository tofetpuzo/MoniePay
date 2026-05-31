namespace MoniePay.src.models
{
    public class PaymentAttempts
    {
        public Guid Id { get; set; }
        public Guid PaymentId { get; set; }
        public int AttemptNumber { get; set; }
        public string PaymentStatus { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public Customer? Customer { get; set; }
        public ICollection<PaymentMethods> PaymentMethods { get; set; } = new List<PaymentMethods>();
        public PaymentAttempts() { }
        public PaymentAttempts(Guid id, Guid paymentId, int attemptNumber,
            string paymentStatus, string errorMessage, DateTime createdAt)
        {
            Id = id;
            PaymentId = paymentId;
            AttemptNumber = attemptNumber;
            PaymentStatus = paymentStatus;
            ErrorMessage = errorMessage;
            CreatedAt = createdAt;
        }
    }
}
