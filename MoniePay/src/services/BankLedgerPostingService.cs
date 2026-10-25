// SPDX-License-Identifier: Apache-2.0
/*
 * Posts one balanced transaction: debit one account, credit another, write
 * both ledger entries and the Transactions row. Does NOT save or commit - the
 * caller owns the database transaction so it can add its own rows atomically.
 *
 * Copyright (c) 2026, MoniePay
 */

using MoniePay.src.data;
using MoniePay.src.models;

namespace MoniePay.src.services
{
    public class BankLedgerPostingService(AppDbContext db)
    {
        private readonly AppDbContext db = db;

        public Transactions Post(
            TransactionType type, LedgerAccounts debitAccount,
            LedgerAccounts creditAccount, decimal amount,
            string reference, Guid? initiateBy,
            string description = "",
            Guid? paymentIntentId = null
            )
        {
            if (amount <= 0) throw new ArgumentException("Amount must be greater than zero", nameof(amount));
            if (debitAccount.Id == creditAccount.Id) throw new InvalidOperationException("cannot post to the same account");

            //check the currency 
            if (debitAccount.Currency != creditAccount.Currency) throw new InvalidOperationException
                    ($"currency mismatch :{debitAccount.Currency} and {creditAccount.Currency}");

            // capital check if bank has money
            if (debitAccount.AccountType != AccountType.Capital && debitAccount.Balance < amount) throw new InvalidOperationException
                    ($"Insufficient amount :{debitAccount.AccountName} and {debitAccount.Balance}");

            var transaction = new Transactions
                (type, Guid.Empty, amount, debitAccount.Currency, reference, initiateBy)
            {
                Description = description,
                PaymentIntentId = paymentIntentId,
                Status = Status.SUCCESS,
            };
            db.Transaction.Add(transaction);
            db.LedgerEntries.AddRange(
                new LedgerEntries(Guid.NewGuid(), debitAccount.Id, -amount, "DEBIT", transaction.Id, transaction.CreatedAt)
                {
                    TransactionId = transaction.Id,
                },
                new LedgerEntries(Guid.NewGuid(), creditAccount.Id, amount, "CREDIT", transaction.Id, transaction.CreatedAt)
                {
                    TransactionId = transaction.Id,
                });

            debitAccount.Balance -= amount;
            creditAccount.Balance += amount;
            return transaction;
        }

        // TODO: create BankingFundingRequest and BankFundingResponse
        // TODO: create BankService to use posting transactions service 
        // TODO: create Bank Controller
        // TODO: update the cashier service and cashier controller
    }
}
