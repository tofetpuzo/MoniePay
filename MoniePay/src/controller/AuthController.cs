// SPDX-License-Identifier: Apache-2.0
/*
 * HTTP endpoints for authentication and user registration.
 *
 * Copyright (c) 2026, MoniePay
 */

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoniePay.src.services;

namespace MoniePay.src.controller
{
    [ApiController]
    [Route("api/[controller]")]
    [IgnoreAntiforgeryToken]
    public class AuthController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public AuthController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<IActionResult> CreateUser(RegisterUser user)
        {
            if (user is null) return BadRequest();
            var res = await _identityService.RegisterUserAsync(user);
            return Ok(res);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(string username, string password)
        {
            if (username == null || password == null) return BadRequest();
            var res = await _identityService.LoginAsync(username, password);
            return Ok(res);
        }

    }
}
