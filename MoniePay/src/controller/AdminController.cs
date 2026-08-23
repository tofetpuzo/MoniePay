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
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [AllowAnonymous]
        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterAdmin(CreateAdminRequest request)
        {
            if (request is null) return BadRequest();
            var created = await _adminService.RegisterAdminAsync(request);
            return Ok(created);
        }

    }
}


