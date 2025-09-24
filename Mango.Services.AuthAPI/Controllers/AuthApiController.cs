using Mango.Services.AuthAPI.Dtos;
using Mango.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.AuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthApiController(IAuthService _authService) : ControllerBase
    {
        protected ResponseDto _responseDto = new();
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegistrationRequestDto model)
        {
            var result = await _authService.Register(model);

            if (!string.IsNullOrEmpty(result))
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = result;
                return BadRequest(_responseDto);
            }


            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto model)
        {
            var loginResponse = await _authService.Login(model);
            if (loginResponse.User is null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Username or Password is incorrect";
                return BadRequest(_responseDto);
            }
            return Ok(loginResponse);
        }
    }
}
