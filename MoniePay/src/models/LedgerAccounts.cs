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
        public AccountType AccountType { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public Currency Currency { get; set; }
        public Guid CustomerId { get; set; }
        public decimal Balance { get; set; } = decimal.Zero;
        public DateTime CreatedAt { get; set; }
        public LedgerAccounts() { }
        public LedgerAccounts(Guid id, string accountName, Currency currency, decimal balance, Guid custId,
            string accountNumber, AccountType accountTypes)
        {
            Id = id;
            AccountName = accountName;
            Currency = currency;
            Balance = balance;
            CreatedAt = DateTime.UtcNow;
            CustomerId = custId;
            AccountNumber = accountNumber;
            AccountType = accountTypes;
        }

        public static explicit operator string(LedgerAccounts v)
        {
            throw new NotImplementedException();
        }
    }
}
