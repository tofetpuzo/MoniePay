// SPDX-License-Identifier: Apache-2.0
/*
 * Outbound view of a payment intent.
 *
 * The PaymentIntents entity carries Customer, Payment, Transaction, Payout and
 * PaymentAttempts navigations. Serialising it would reach Customer -> user
 * (Identity) and cycle on Customer <-> PaymentMethods, so it is never returned
 * directly. This carries the Id the caller needs and nothing else.
 *
 * Copyright (c) 2026, MoniePay
 */


using Microsoft.EntityFrameworkCore;
using MoniePay.src.data;
using MoniePay.src.dto;

namespace MoniePay.src.services
{

    public interface ICheckCustomerAccountService
    {
        Task<CreateDepositRequest> CheckCustomerDetails(string accountNumber);
    }
    public class CheckCustomerAccountService(AppDbContext db)
    {
        private readonly AppDbContext _db = db;

        // get customer account number
        public async Task<string> GetCustomerAccountNumberAsync(
            string customerNumber,
            CancellationToken cancellationToken = default)
        {
            var account = await db.LedgerAccount
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    a => a.AccountNumber == customerNumber,
                    cancellationToken);

            return account?.AccountNumber
                ?? throw new KeyNotFoundException("Account number not found.");
        }

    }
}
