namespace MoniePay.src.models
{
    public class PaymentMethods
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string Last4 { get; set; } = string.Empty;
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public DateTime CreatedAt { get; set; }
        public Customer? Customer { get; set; }
        public PaymentMethods() { }

        public PaymentMethods(Guid id, Guid customerId, string type, string token, string last4, int expiryMonth, int expiryYear, DateTime createdAt)
        {
            Id = id;
            CustomerId = customerId;
            Type = type;
            Token = token;
            Last4 = last4;
            ExpiryMonth = expiryMonth;
            ExpiryYear = expiryYear;
            CreatedAt = createdAt;
        }
    }
}
