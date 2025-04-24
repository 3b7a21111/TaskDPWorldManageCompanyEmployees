using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Core.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }
        [HttpPost("login")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Login (UserLoginDTO loginDTO)
        {
            if(!ModelState.IsValid)
                return BadRequest("You Must Enter UserName & Password ");
            var result = await authService.LoginAsync(loginDTO);
            if (result == null)
                return Unauthorized("Invalid username or password");
            return Ok(result);
        }
    }
}
