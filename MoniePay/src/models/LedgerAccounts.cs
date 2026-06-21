// SPDX-License-Identifier: Apache-2.0
/*
 * Chart of accounts for the double-entry ledger.
 *
 * Copyright (c) 2026, MoniePay
 */

namespace MoniePay.src.models
{
    public class LedgerAccounts
    {
        public Guid Id { get; set; }
        public string AccountName { get; set; } = string.Empty;
        public string AccountType { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public LedgerAccounts() { }
        public LedgerAccounts(Guid id, string accountName, string currency)
        {
            Id = id;
            AccountName = accountName;
            Currency = currency;
        }
    }
}
