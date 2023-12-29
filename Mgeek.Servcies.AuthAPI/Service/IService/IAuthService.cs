using Mgeek.Servcies.AuthAPI.Models.DTO;

namespace Mgeek.Servcies.AuthAPI.Service.IService;

public interface IAuthService
{
    Task<string> Register(RegistrationRequestDto registrationRequestDto);
    Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
    Task<bool> AssignRole(string email, string rolename);
}