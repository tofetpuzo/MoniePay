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

namespace MoniePay.src.services
{

    public interface IDepositService
    {

        Task<CreateDepositRequest> CreatePaymentIntent(CreateDepositRequest? request, Guid authenticatedUserId);
    }

    public class DepositService(UserManager<User> userManager, AppDbContext db)
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly AppDbContext _db = db;


        // logic for counter deposit 
        public async Task<DepositResponse> CallCashierLibrary(CreateDepositRequest createDepositRequest)
        {
            if (createDepositRequest == null)
            {
                throw new ArgumentNullException(nameof(createDepositRequest));
            }


            if (createDepositRequest.channel == Channel.COUNTER && createDepositRequest.Receiver != null)
            {
                // fetch cashier details 
                var cashier = await _db.Users.FirstOrDefaultAsync(
                    u => u.UserName == createDepositRequest.Receiver.UserName && u.RoleFlags.Equals(128));

                // cashier is null 
                if (cashier == null) throw new KeyNotFoundException("unauthorized request");

                if (cashier != null)
                {
                    createDepositRequest.Receiver = cashier;
                }

                // library call needs to be implement for await ...
                return DepositResponse.From(createDepositRequest);
            }

            throw new ArgumentException("Unsupported deposit channel.", nameof(createDepositRequest.channel));
        }

        // check depositor account



        // import ATM library

    }
}
