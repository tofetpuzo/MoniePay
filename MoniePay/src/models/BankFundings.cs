// SPDX-License-Identifier: Apache-2.0
/*
 * Money arriving in one of our real bank accounts. Recorded by one finance
 * user, confirmed by a different one (maker-checker) before it hits the ledger.
 *
 * Copyright (c) 2026, MoniePay
 */

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
