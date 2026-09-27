// SPDX-License-Identifier: Apache-2.0
/*
 * Customer onboarding: creates the Identity user and the customer profile
 * atomically.
 *
 * Copyright (c) 2026, MoniePay
 */

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MoniePay.src.auth;
using MoniePay.src.data;
using MoniePay.src.dto;
using MoniePay.src.models;

namespace MoniePay.src.services
{

    public interface IDepositService
    {
        Task<DepositResponse> CallCashierLibrary(CreateDepositRequest request, Guid authenticateId);
    }

    public class DepositService(UserManager<User> userManager,
        AppDbContext db,
        CheckCustomerAccountService checkCustomerAccountService) : IDepositService
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly AppDbContext _db = db;
        private readonly CheckCustomerAccountService _checkCustomerAccountService = checkCustomerAccountService;


        // logic for counter deposit 
        public async Task<DepositResponse> CallCashierLibrary(CreateDepositRequest createDepositRequest, Guid authenticateId)
        {
            if (createDepositRequest == null)
            {
                throw new ArgumentNullException(nameof(createDepositRequest), "Parameter cannot be null.");
            }
            if (authenticateId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(authenticateId), "Parameter cannot be empty.");
            }

            if (createDepositRequest.channel == Channel.COUNTER && createDepositRequest.Receiver != null)
            {
                // fetch cashier details 
                var cashier = await _db.Users.FirstOrDefaultAsync(
                    u => u.UserName == createDepositRequest.Receiver.UserName && u.RoleFlags.Equals(128) && (u.isActive == true));

                // cashier is null 
                if (cashier == null) throw new KeyNotFoundException("unauthorized request");

                if (cashier != null)
                {
                    createDepositRequest.Receiver = new CashierDTO()
                    {
                        Id = cashier.Id,
                        UserName = cashier.UserName ?? string.Empty,
                    };
                }

                try
                {
                    // begin transaction 
                    await using var transaction = await _db.Database.BeginTransactionAsync();

                    // get account number 
                    var account = await _checkCustomerAccountService.GetCustomerAccountNumberAsync(
                                                    createDepositRequest.DestinationAccountNumber);

                    // retrieve verification details
                    bool verifiedCustomer = await _checkCustomerAccountService.IsCustomerVerified(account.customerId);

                    if (!verifiedCustomer) throw new KeyNotFoundException("User is not verified");

                    // credit depositor
                    account.Balance += createDepositRequest.Amount;

                    // Transaction record for the settlement 
                    var customerTransaction = new Transactions(Guid.NewGuid(), Guid.Empty, DateTime.UtcNow);
                    _db.Transaction.Add(customerTransaction);

                    //Enter the ledger entries for both the cashier and depositor
                    _db.LedgerEntries.AddRange(
                    new LedgerEntries(Guid.NewGuid(), cashier!.Id, createDepositRequest.Amount, "CASH", account.customerId, DateTime.UtcNow)
                    {
                        TransactionId = customerTransaction.Id,
                    },
                    new LedgerEntries(Guid.NewGuid(), account.Id, createDepositRequest.Amount, "CASH", account.customerId, DateTime.UtcNow)
                    {
                        TransactionId = customerTransaction.Id,
                    });

                    await _db.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await _db.Database.RollbackTransactionAsync();
                    throw new Exception("Cannot process customer transaction ", ex);
                }

                return DepositResponse.From(createDepositRequest);
            }

            throw new ArgumentException("Unsupported deposit channel.", nameof(createDepositRequest));
        }

        // TODO: create endpoint for different users( e.g - cashier etc)

        // TODO: Test code for deposit endpoint
    }
}
