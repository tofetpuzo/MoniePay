// SPDX-License-Identifier: Apache-2.0
/*
 * UserService onboarding: user controller for moniepay
 * Copyright (c) 2026, MoniePay
 */


using MoniePay.src.auth;
using MoniePay.src.dto;
using MoniePay.src.shared;

namespace MoniePay.src.services
{
    public class CashierService(RegisterUserService registerUserService)
    {
        private readonly RegisterUserService _registerUser = registerUserService;

        // register user service
        public async Task<UserResponse> RegisterCashierAsync(CreateUserRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            UserResponse userResponse = null;

            if (request.RoleFlags == Roles.RoleType.Cashier)
            {
                var cashier = await _registerUser.RegisterUserAsync(request, Roles.RoleType.Cashier);
                if (cashier == null) throw new ArgumentNullException(nameof(cashier));
            }
            userResponse = UserResponse.From(request);
            userResponse.Status = Status.SUCCESS;

            return userResponse;
        }
    }
}