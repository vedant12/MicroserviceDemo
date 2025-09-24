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
        public async Task<UserDto> Register(RegistrationRequestDto model)
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

                    return userDto;
                }
            }
            catch (Exception ex)
            {
            }

            return new UserDto();
        }

        public Task<LoginResponseDto> Login(LoginRequestDto model)
        {
            throw new NotImplementedException();
        }
    }
}
