// SPDX-License-Identifier: Apache-2.0
/*
 * Cashier Controller: cashier controller use to pay cashier  
 * atomically.
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
    public class CashierController(CashierService cashierService) : ControllerBase
    {
        private readonly CashierService _cashier = cashierService;


        [AllowAnonymous]
        [HttpPost("fund-cashier")]
        public async Task<IActionResult> FundCashier(FundingCashierRequest request, Guid fundedBy)
        {
            try
            {
                return Ok(await _cashier.FundCashierAccount(request, fundedBy);

            }

        }

    }
}
