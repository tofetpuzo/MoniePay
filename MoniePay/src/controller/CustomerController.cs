// SPDX-License-Identifier: Apache-2.0
/*
 * HTTP endpoints for customer onboarding.
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
    [IgnoreAntiforgeryToken]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [AllowAnonymous]
        [HttpPost("register-customer")]
        public async Task<IActionResult> RegisterCustomer(RegisterCustomerRequest request)
        {
            if (request is null) return BadRequest();
            var created = await _customerService.RegisterCustomerAsync(request);
            return Ok(created);
        }
    }
}
