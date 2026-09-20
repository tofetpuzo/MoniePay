// SPDX-License-Identifier: Apache-2.0
/*
 * use by many services(ATM, Counter, API and POS for checking customer account
 *
 * Copyright (c) 2026, MoniePay
 */

using Microsoft.EntityFrameworkCore;
using MoniePay.src.data;
using MoniePay.src.dto;
using MoniePay.src.models;

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
        public async Task<LedgerAccounts> GetCustomerAccountNumberAsync(
            string customerNumber,
            CancellationToken cancellationToken = default)
        {
            var account = await _db.LedgerAccount
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    a => a.AccountNumber == customerNumber,
                    cancellationToken);

            return account
                ?? throw new KeyNotFoundException("Account number not found.");
        }

        // get customer data 
        public async Task<Boolean> IsCustomerVerified(Guid customerNumberId,
            CancellationToken cancellationToken = default)
        {
            var verified = await _db.Customers.AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == customerNumberId, cancellationToken);

            return verified != null && verified.IsVerifed;
        }
    }
}
