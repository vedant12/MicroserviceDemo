using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class AuthController(IAuthService _authService) : Controller
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
                    //LoginResponseDto loginResponseDto =
                    //    JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(responseDto.Result));

                    //await SignInUser(loginResponseDto);
                    //_tokenProvider.SetToken(loginResponseDto.Token);
                    //return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["error"] = responseDto.Message;
                }
            }
            else
            {

            }
            return View(model);
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
