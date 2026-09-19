// SPDX-License-Identifier: Apache-2.0
/*
 * use by many services(ATM, Counter, API and POS for checking customer account
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
        Task<CreateDepositRequest> GetCustomerAccountNumberAsync(string accountNumber);
    }
    public class CheckCustomerAccountService(AppDbContext db)
    {
        private readonly AppDbContext _db = db;

        // get customer account number
        public async Task<string> GetCustomerAccountNumberAsync(
            string customerNumber,
            CancellationToken cancellationToken = default)
        {
            var account = await _db.LedgerAccount
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    a => a.AccountNumber == customerNumber,
                    cancellationToken);

            return account?.AccountNumber
                ?? throw new KeyNotFoundException("Account number not found.");
        }

    }
}
