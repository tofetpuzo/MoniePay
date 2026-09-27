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
    public class DepositController(DepositService _depositService) : ControllerBase
    {
        private readonly DepositService depositService = _depositService;

        [AllowAnonymous]
        [HttpPost("deposit-customer")]
        public async Task<IActionResult> CustomerDeposit(CreateDepositRequest request, Guid authenticateId)
        {
            if (request is null) return BadRequest();
            var created = await depositService.CallCashierLibrary(request, authenticateId);
            return Ok(created);
        }
    }
}