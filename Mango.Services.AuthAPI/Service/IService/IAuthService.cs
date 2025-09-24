using Mango.Services.AuthAPI.Dtos;

namespace Mango.Services.AuthAPI.Service.IService
{
    public interface IAuthService
    {
        Task<UserDto> Register(RegistrationRequestDto model);
        Task<LoginResponseDto> Login(LoginRequestDto model);
    }
}
