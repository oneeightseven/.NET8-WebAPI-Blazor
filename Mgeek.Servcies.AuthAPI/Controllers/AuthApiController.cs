using Mgeek.Servcies.AuthAPI.Models.DTO;
using Mgeek.Servcies.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace Mgeek.Servcies.AuthAPI.Controllers;

[Route("api/AuthAPI")]
[ApiController]
public class AuthApiController : ControllerBase
{
    private ResponseDto _response;
    private readonly IAuthService _authService;
    
    public AuthApiController(IAuthService authService)
    {
        _authService = authService;
        _response = new();
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegistrationRequestDto model)
    {
        var result = await _authService.Register(model);
        if (result != "Succeeded")
        {
            _response.Message = result;
            _response.IsSuccess = false;
            return BadRequest(_response);
        }
        return Ok(_response);
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
    {
        var loginResponse = await _authService.Login(model);
        if (loginResponse.User == null)
        {
            _response.IsSuccess = false;
            _response.Message = "Username or password is incorrect";
            return BadRequest(_response);
        }
        _response.Result = loginResponse;
        return Ok(_response);
    }
    
    [HttpPost("assignRole")]
    public async Task<IActionResult> AssignRole([FromBody] RegistrationRequestDto model)
    {
        var loginResponse = await _authService.AssignRole(model.Email!, model.Role!.ToUpper());
        if (!loginResponse)
        {
            _response.IsSuccess = false;
            _response.Message = "Error encountered";
            return BadRequest(_response);
        }
        return Ok(_response);
    }
    
}