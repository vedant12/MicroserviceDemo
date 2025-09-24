using Mango.Services.AuthAPI.Data;
using Mango.Services.AuthAPI.Dtos;
using Mango.Services.AuthAPI.Models;
using Mango.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.AuthAPI.Service
{
    public class AuthService(AppDbContext _context,
            UserManager<ApplicationUser> _userManager,
            RoleManager<IdentityRole> _roleManager) : IAuthService
    {
        public async Task<string> Register(RegistrationRequestDto model)
        {
            ApplicationUser user = new()
            {
                UserName = model.Email,
                Email = model.Email,
                NormalizedEmail = model.Email.ToUpper(),
                Name = model.Name,
                PhoneNumber = model.PhoneNumber
            };

            try
            {
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var dbUser = await _context.ApplicationUsers.FirstOrDefaultAsync(x => x.UserName == model.Email);

                    UserDto userDto = new()
                    {
                        Name = dbUser.Name,
                        Email = dbUser.Email,
                        PhoneNumber = dbUser.PhoneNumber,
                        Id = dbUser.Id
                    };

                    return "";
                }
                else
                {
                    return result.Errors.FirstOrDefault().Description;
                }
            }
            catch (Exception ex)
            {
            }

            return "Error Encountered";
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto model)
        {
            var dbUser = await _context.ApplicationUsers.FirstOrDefaultAsync(x => x.UserName == model.UserName);

            bool isValid = await _userManager.CheckPasswordAsync(dbUser, model.Password);

            if(dbUser is null || !isValid)
            {
                return new LoginResponseDto() { User = null, Token = String.Empty };
            }

            UserDto userDTO = new()
            {
                Email = dbUser.Email,
                Id = dbUser.Id,
                Name = dbUser.Name,
                PhoneNumber = dbUser.PhoneNumber
            };

            return new LoginResponseDto() { User = userDTO, Token = "" };
        }
    }
}
