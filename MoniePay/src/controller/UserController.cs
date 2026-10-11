// SPDX-License-Identifier: Apache-2.0
/*
 * UserService onboarding: user controller for moniepay
 * Copyright (c) 2026, MoniePay
 */

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoniePay.src.auth;
using MoniePay.src.dto;
using MoniePay.src.shared;

namespace MoniePay.src.controller
{

    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    [IgnoreAntiforgeryToken]
    public class UserController(UserService registerUserService) : ControllerBase
    {
        private readonly UserService _registerUserService = registerUserService;

        [AllowAnonymous]
        [HttpPost("register-user")]
        public async Task<IActionResult> RegisterUser(CreateUserRequest request, Roles.RoleType roleType)
        {
            if (request is null) return BadRequest();
            if (roleType != Roles.RoleType.Cashier) throw new KeyNotFoundException("cannot create cashier role");
            var created = await _registerUserService.RegisterUserAccountAsync(request, roleType);
            return Ok(created);
        }
    }
}
