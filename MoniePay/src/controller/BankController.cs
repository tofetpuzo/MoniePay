// SPDX-License-Identifier: Apache-2.0
/*
 * Banking Controller which is used to send requests
 *
 * Copyright (c) 2026, MoniePay
 */

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoniePay.src.dto;
using MoniePay.src.services;
using System.Security.Claims;

namespace MoniePay.src.controller
{
    [ApiController]
    [Route("api/[controller]")]
    //[AllowAnonymous]
    [IgnoreAntiforgeryToken]
    [Authorize(Roles = "Admin")]
    public class BankController(BankFundingService bankFundingService) : ControllerBase
    {
        private readonly BankFundingService _bankService = bankFundingService;

        [AllowAnonymous]
        [HttpPost("funding")]
        public async Task<IActionResult> Record(BankingFundingRequest banking)
            => await Run(userId => _bankService.RecordFunding(banking, userId));

        [HttpPost("funding/{id:guid}/confirm")]
        public async Task<IActionResult> Confirm(Guid id)
            => await Run(userId => _bankService.PostFunding(id, userId));

        [HttpPost("fund-vault")]
        public async Task<IActionResult> ToVault(TransferToVaultRequest transferTo)
            => await Run(async userId => new { tranasctionId = await _bankService.TransferToVaultAsync(transferTo, userId) });

        private async Task<IActionResult> Run<T>(Func<Guid, Task<T>> action)
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier); // use your user-ID claim type

            if (!Guid.TryParse(userIdValue, out var userId))
                return Unauthorized();

            var result = await action(userId);
            return Ok(result);
        }
    }
}
