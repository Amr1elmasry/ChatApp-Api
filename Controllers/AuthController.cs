using ChatApp_Api.DTOS.Auth;
using ChatApp_Api.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("UserRegister")] 
        public async Task<IActionResult> UserRegister(UserRegisterDto userRegisterDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.UserRegister(userRegisterDto);

            if (!result.IsAuth)
                return BadRequest(result.Massage);
            return Ok(result);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto )
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.UserLogin(userLoginDto);

            if (!result.IsAuth)
                return BadRequest(result.Massage);

            return Ok(result);
        }
    }
}
