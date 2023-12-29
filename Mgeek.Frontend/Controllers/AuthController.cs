using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Mgeek.Frontend.Models.AuthAPI;
using Mgeek.Frontend.Service.IService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mgeek.Frontend.Controllers;

public class AuthController : Controller
{
    private readonly IAuthService _authService;
    private readonly ITokenProvider _tokenProvider;
    
    public AuthController(IAuthService authService, ITokenProvider tokenProvider)
    {
        _authService = authService;
        _tokenProvider = tokenProvider;
    }
    
    [HttpGet]
    public IActionResult Login()
    {
        LoginRequestDto loginRequestDto = new();
        return View(loginRequestDto);
    }
    
    [HttpPost]
    public async Task<IActionResult> Login(LoginRequestDto model)
    {
        var responseDto = await _authService.LoginAsync(model);
        if (responseDto!.IsSuccess)
        {
            var loginResponseDto =
                JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(responseDto.Result)!);
            
            await SignInUser(loginResponseDto!);
            _tokenProvider.SetToken(loginResponseDto!.Token!);
            return RedirectToAction("Index", "Home");
        }
        ViewBag.Message = responseDto!.Message! + "*";
        ModelState.AddModelError("AuthError", responseDto!.Message!);
        return View(model);
    } 
    
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Register(RegistrationRequestDto model)
    {
        var responseDto = await _authService.RegisterAsync(model);
        if (responseDto!.IsSuccess)
        {
            var assignRole = await _authService.AssignRoleAsync(model);
            if (assignRole!.IsSuccess)
            {
                return RedirectToAction(nameof(Login));
            }
        }
        ViewBag.Message = responseDto!.Message! + "*";
        return View("Register");
    }
    
    [HttpGet]
    public IActionResult PersonalArea()
    {
        return View();
    }
    
    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        _tokenProvider.ClearToken();
        return RedirectToAction("Index", "Home");
    }

    private async Task SignInUser(LoginResponseDto model)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(model.Token);

        var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
        identity.AddClaim(new (JwtRegisteredClaimNames.Email, jwt.Claims.FirstOrDefault(u=>u.Type== JwtRegisteredClaimNames.Email)!.Value));
        identity.AddClaim(new (JwtRegisteredClaimNames.Sub, jwt.Claims.FirstOrDefault(u=>u.Type== JwtRegisteredClaimNames.Sub)!.Value));
        identity.AddClaim(new (JwtRegisteredClaimNames.Name, jwt.Claims.FirstOrDefault(u=>u.Type== JwtRegisteredClaimNames.Name)!.Value));
        
        identity.AddClaim(new (ClaimTypes.Name, jwt.Claims.FirstOrDefault(u=>u.Type== JwtRegisteredClaimNames.Email)!.Value));
        identity.AddClaim(new (ClaimTypes.Role, jwt.Claims.FirstOrDefault(u=>u.Type== "role")!.Value));
        
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
    }
}