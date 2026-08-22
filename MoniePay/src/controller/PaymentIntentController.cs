using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoniePay.src.dto;
using MoniePay.src.services;
using System.Security.Claims;

namespace MoniePay.src.controller
{

    [ApiController]
    [Route("api/[controller]")]
    [IgnoreAntiforgeryToken]
    [Authorize] // every action requires a valid JWT unless it opts out
    public class PaymentIntentController : ControllerBase
    {

        private readonly IPaymentIntentService _paymentIntentService;

        public PaymentIntentController(IPaymentIntentService ipayementservice)
        {
            _paymentIntentService = ipayementservice;
        }

        // Was [AllowAnonymous]: anyone on the internet could move money.
        [HttpPost("mpay")]
        public async Task<IActionResult> PayCustomer(CreatePaymentIntentRequest request)
        {
            if (request is null) return BadRequest();

            // Identity of the payer comes from the signed token, never the body.
            var subject = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(subject, out var authenticatedUserId))
                return Unauthorized();

            try
            {
                var paymentIntent = await _paymentIntentService
                    .CreatePaymentIntent(request, authenticatedUserId);
                return Ok(paymentIntent);
            }
            catch (UnauthorizedAccessException ex)
            {
                // Paying from someone else's customer account.
                return Forbid(ex.Message);
            }
        }
    }

}
