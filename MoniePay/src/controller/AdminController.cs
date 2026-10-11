// SPDX-License-Identifier: Apache-2.0
/*
 * Customer onboarding: creates the Identity user and the customer profile
 * atomically.
 *
 * Copyright (c) 2026, MoniePay
 */


using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoniePay.src.dto;
using MoniePay.src.services;

namespace MoniePay.src.controller
{

    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    [IgnoreAntiforgeryToken]
    public class AdminController(IAdminService adminService, CashierService cashierService) : ControllerBase
    {
        private readonly IAdminService _adminService = adminService;
        private readonly CashierService _cashierService = cashierService;

        [AllowAnonymous]
        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterAdmin(CreateUserRequest request)
        {
            // set admin role by default
            request.RoleFlags = auth.Roles.RoleType.Admin;

            if (request is null) return BadRequest();
            var created = await _adminService.RegisterAdminAsync(request);
            return Ok(created);
        }


        [AllowAnonymous]
        [HttpPost("register-cashier")]
        public async Task<IActionResult> RegisterNewCashier(CreateUserRequest request)
        {
            // set cashier role by default
            request.RoleFlags = auth.Roles.RoleType.Cashier;

            if (request is null) return BadRequest();
            var cashierCreated = await _cashierService.RegisterCashierAsync(request);
            return Ok(cashierCreated);
        }
    }
}


