// SPDX-License-Identifier: Apache-2.0
/*
 * CashierService onboarding: cashier service for moniepay
 * Copyright (c) 2026, MoniePay
 */


using MoniePay.src.auth;
using MoniePay.src.data;
using MoniePay.src.dto;
using MoniePay.src.shared;

namespace MoniePay.src.services
{
    public class CashierService(UserService userService, AppDbContext db,
        LedgerAccountService ledgerAccount)
    {
        private readonly UserService _sharedUserService = userService;
        private readonly LedgerAccountService _ledger = ledgerAccount;
        private readonly AppDbContext _db = db;


        // register user service
        public async Task<UserResponse> RegisterCashierAsync(CreateUserRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            if (request.RoleFlags == Roles.RoleType.Cashier)
            {
                var cashier = await _sharedUserService.RegisterUserAccountAsync(request, Roles.RoleType.Cashier);
                if (cashier == null) throw new ArgumentNullException(nameof(cashier));
            }
            var userResponse = UserResponse.From(request);
            userResponse.Status = Status.SUCCESS;

            return userResponse;
        }

        // fund the cashier account 
        public async Task<CashierResponse> FundCashierAccount(FundingCashierRequest cashierDTO, Guid fundedBy)
        {
            ArgumentNullException.ThrowIfNull(cashierDTO);

            // check amount is not negative
            if (cashierDTO.Amount <= 0) throw new ArgumentNullException(nameof(cashierDTO));

            // check cashier
            var cashier = await _db.Users.FindAsync(cashierDTO.CashierId)
                ?? throw new KeyNotFoundException($"Cashier {cashierDTO.CashierId} not found");

            if (!cashier.RoleFlags.HasFlag(Roles.RoleType.Cashier)) throw new KeyNotFoundException(
                nameof(cashierDTO));

            if (cashier.isActive != true) throw new KeyNotFoundException(nameof(cashier));

            // check for idempotency.

            // check the bank-ledger amount if there is enough cash 

            // check the cashier account to see if they are active and if their account < 0

            // add to global transaction.

            return CashierResponse.From(cashierDTO);
        }
    }
}
