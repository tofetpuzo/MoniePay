namespace MoniePay.src.models
{
    public class BankAccounts
    {
        public Guid Id { get; set; }
        public string BankName { get; set; } = string.Empty;
        public string AccountName { get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty;
        public string SortCode { get; set; } = string.Empty;
        public string SwiftCode { get; set; } = string.Empty;
        public Currency Currency { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public Guid LedgerAcountId { get; set; }
        public LedgerAccounts? LedgerAccounts { get; set; }
        public ICollection<BankFundings> Fundings { get; set; } = [];
    }
}
