namespace MoniePay.src.models
{
    public class LedgerEntries
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public decimal Amount { get; set; }
        public string EntryType { get; set; } = string.Empty;
        public Guid ReferenceId { get; set; }
        public DateTime CreatedAt { get; set; }

        public LedgerAccounts? LedgerAccount { get; set; }

        public LedgerEntries() { }

        public LedgerEntries(Guid id, Guid accountId, decimal amount, string entryType, Guid referenceId, DateTime createdAt)
        {
            Id = id;
            AccountId = accountId;
            Amount = amount;
            EntryType = entryType;
            ReferenceId = referenceId;
            CreatedAt = createdAt;
        }
    }
}
