using Mgeek.Frontend.Models;
using Mgeek.Frontend.Models.AuthAPI;
using Mgeek.Frontend.Service.IService;
using Mgeek.Frontend.Utility;

namespace Mgeek.Frontend.Service;

public class AuthService : IAuthService
{
    private readonly IBaseService _baseService;
    public AuthService(IBaseService baseService)
    {
        _baseService = baseService;
    }
    public async Task<ResponseDto?> LoginAsync(LoginRequestDto loginRequestDto)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = WC.ApiType.POST,
            Data = loginRequestDto,
            Url = WC.AuthApiBase + "/api/AuthAPI/login"
        },withBearer: false );
    }

    public async Task<ResponseDto?> RegisterAsync(RegistrationRequestDto registrationRequestDto)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = WC.ApiType.POST,
            Data = registrationRequestDto,
            Url = WC.AuthApiBase + "/api/AuthAPI/register"
        },withBearer: false );
    }

    public async Task<ResponseDto?> AssignRoleAsync(RegistrationRequestDto registrationRequestDto)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = WC.ApiType.POST,
            Data = registrationRequestDto,
            Url = WC.AuthApiBase + "/api/AuthAPI/assignRole"
        });
    }
}