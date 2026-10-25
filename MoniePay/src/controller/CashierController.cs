// SPDX-License-Identifier: Apache-2.0
/*
 * Cashier Controller: cashier controller use to pay cashier  
 * atomically.
 * Copyright (c) 2026, MoniePay
 */

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MoniePay.src.controller
{

    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    [IgnoreAntiforgeryToken]
    public class CashierController : ControllerBase
    {

    }
}
