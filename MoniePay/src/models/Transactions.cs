// SPDX-License-Identifier: Apache-2.0
/*
 * One balanced movement of money. Every transaction owns exactly the ledger
 * entries that make it up, and its Type says what caused it.
 *
 * Copyright (c) 2026, MoniePay
 */

namespace MoniePay.src.models
{
    public class Transactions
    {
        public Guid Id { get; set; }
        public TransactionType Type { get; set; }
        // only for customer payments 
        public Guid? PaymentIntentId { get; set; } = Guid.Empty;
        public decimal Amount { get; set; }
        public Currency Currency { get; set; }
        public Status Status { get; set; } = Status.PROCESSING;
        public string Reference { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        //the id of who performed the transaction
        public Guid? InitiatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<LedgerEntries> LedgerEntries { get; set; } = new List<LedgerEntries>();

        public Transactions() { }

        public Transactions(TransactionType type, decimal amount, Currency currency, string reference, Guid? initiatedBy)
        {
            Id = Guid.CreateVersion7();
            Type = type;
            Amount = amount;
            Currency = currency;
            Reference = reference;
            InitiatedBy = initiatedBy;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
