using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Mango.Web.Controllers
{
    public class AuthController(IAuthService _authService, ITokenProvider _tokenProvider) : Controller
    {
        public IActionResult Login()
        {
            return View(new LoginRequestDto());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto model)
        {
            if (ModelState.IsValid)
            {
                ResponseDto responseDto = await _authService.LoginAsync(model);

                if (responseDto != null && responseDto.IsSuccess)
                {
                    LoginResponseDto loginResponseDto = JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(responseDto.Result))!;

                    await SignInUser(loginResponseDto);
                    
                    _tokenProvider.SetToken(loginResponseDto.Token);
                    
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("CustomError", responseDto?.Message);
                }
            }
            else
            {
                ModelState.AddModelError("CustomError", "Invalid Model Data");
            }
            return View(model);
        }

        private async Task SignInUser(LoginResponseDto? loginResponseDto)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var jwtToken = tokenHandler.ReadJwtToken(loginResponseDto.Token);

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email,
                jwtToken.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email)!.Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub,
                jwtToken.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub)!.Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name,
                jwtToken.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name)!.Value));

            identity.AddClaim(new Claim(ClaimTypes.Name,
                jwtToken.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email)!.Value));

            if (jwtToken.Claims.FirstOrDefault(x => x.Type == "role") != null)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role,
                    jwtToken.Claims.FirstOrDefault(u => u.Type == "role")!.Value));
            }

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

        public IActionResult Register()
        {
            PopulateDropDowns();

            return View(new RegistrationRequestDto());
        }

        private void PopulateDropDowns()
        {
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem{ Text = SD.RoleAdmin, Value = SD.RoleAdmin },
                new SelectListItem{ Text = SD.RoleCustomer, Value = SD.RoleCustomer },
            };

            ViewBag.RoleList = roleList;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationRequestDto model)
        {
            ResponseDto result = await _authService.RegisterAsync(model);
            ResponseDto assingRole;

            if (result != null && result.IsSuccess)
            {
                if (string.IsNullOrEmpty(model.Role)) model.Role = SD.RoleCustomer;

                assingRole = await _authService.AssignRoleAsync(model);

                if (assingRole != null && assingRole.IsSuccess)
                {
                    TempData["success"] = "Registration successful";
                    return RedirectToAction("Login");
                }
            }
            else
            {
                TempData["error"] = result?.Message;
            }

            PopulateDropDowns();

            return View(model);
        }
    }
}
