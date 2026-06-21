using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoniePay.src.services;

namespace MoniePay.src.controller
{
    public class AuthController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public AuthController(IIdentityService identityService)
        {
            _identityService = identityService;

        }

        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] RegisterUser user)
        {
            Console.WriteLine("I got here");
            if (user == null) new ArgumentException(nameof(user));
            if (user != null)
            {
                var res = await _identityService.RegisterUserAsync(user);
                //res.Wait();
                return Ok(res);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}