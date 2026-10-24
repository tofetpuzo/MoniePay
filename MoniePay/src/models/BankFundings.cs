namespace MoniePay.src.models
{
    public class BankFundings
    {
        public Guid Id { get; set; }
        public Guid BankAccountId { get; set; }
        public BankAccounts? BankAccount { get; set; }
        public decimal Amount { get; set; }
        public Currency Currency { get; set; }
        public string BankReference { get; set; } = string.Empty;
        public string Narration { get; set; } = string.Empty;
        public Status Status { get; set; } = Status.PROCESSING;
        public Guid RecordedBy { get; set; }
        public Guid ConfirmedBy { get; set; }
        public Guid? TransactionId { get; set; }
        public Transactions? Transactions { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ConfirmedAt { get; set; }
    }
}
