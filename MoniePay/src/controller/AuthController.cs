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
    }
}
