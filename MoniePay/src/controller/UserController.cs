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
    public class UserController(CreateUserRequest user, RegisterUserService registerUserService) : ControllerBase
    {
        private readonly CreateUserRequest _user = user;
        private readonly RegisterUserService _registerUserService = registerUserService;

        [AllowAnonymous]
        [HttpPost("register-user")]
        public async Task<IActionResult> RegisterUser(CreateUserRequest request, Roles.RoleType roleType)
        {
            if (request is null) return BadRequest();
            var created = await _registerUserService.RegisterUserAsync(request, roleType);
            return Ok(created);
        }
    }
}
