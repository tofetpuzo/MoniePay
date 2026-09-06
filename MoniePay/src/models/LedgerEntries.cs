// SPDX-License-Identifier: Apache-2.0
/*
 * Debit and credit postings against ledger accounts.
 *
 * Copyright (c) 2026, MoniePay
 */

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

        // AccountId is the foreign key for this navigation - see AppDbContext.
        // Without that mapping EF invents a second nullable column
        // (LedgerAccountId) and AccountId stops being a real FK, so the
        // navigation is deliberately not a constructor parameter.
        public LedgerAccounts? LedgerAccount { get; set; }

        // add the transaction id 
        public Guid? TransactionId { get; set; }
        public Transactions? Transaction { get; set; }

        public LedgerEntries() { }

        public LedgerEntries(Guid id, Guid accountId, decimal amount, string entryType,
            Guid referenceId, DateTime createdAt)
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
