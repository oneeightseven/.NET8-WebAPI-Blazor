namespace Mgeek.Servcies.AuthAPI.Service;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    public AuthService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IJwtTokenGenerator jwtTokenGenerator)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtTokenGenerator = jwtTokenGenerator;
    }
    public async Task<string> Register(RegistrationRequestDto registrationRequestDto)
    {
        ApplicationUser user = new()
        {
            UserName = registrationRequestDto.Email,
            Email = registrationRequestDto.Email,
            NormalizedEmail = registrationRequestDto.Email!.ToUpper(),
            Name = registrationRequestDto.Name,
            PhoneNumber = registrationRequestDto.PhoneNumber
        };
        try
        {
            var result = await _userManager.CreateAsync(user, registrationRequestDto.Password!);
            if (result.Succeeded)
            {
                var userToReturn =
                    await _dbContext.ApplicationUsers.FirstOrDefaultAsync(x =>
                        x.UserName == registrationRequestDto.Email);
              return "Succeeded";
            }
            else
                return result.Errors.FirstOrDefault()!.Description;
        }
        catch (Exception ex)
        {
            return "Invalid registration request" + ex.Message;
        }
    }
    public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
    {
        var user = await _dbContext.ApplicationUsers.FirstOrDefaultAsync(x =>
            x.UserName!.ToLower() == loginRequestDto.UserName!.ToLower());
        bool isValid = await _userManager.CheckPasswordAsync(user!, loginRequestDto.Password!);
        
        if (user == null || isValid == false)
            return new LoginResponseDto() { User = null, Token = "" };

        UserDto userDto = new()
        {
            Email = user.Email,
            Id = user.Id,
            Name = user.Name,
            PhoneNumber = user.PhoneNumber
        };
        var userRole = await _userManager.GetRolesAsync(user);
        var token = _jwtTokenGenerator.GenerateToken(user,userRole);
        
        return new LoginResponseDto()
        {
            User = userDto,
            Token = token
        };
    }
    public async Task<bool> AssignRole(string email, string rolename)
    {
        var user = await _dbContext.ApplicationUsers.FirstOrDefaultAsync(x => x.Email!.ToLower() == email.ToLower());
        if (user != null)
        {
            if (!_roleManager.RoleExistsAsync(rolename).GetAwaiter().GetResult())
                _roleManager.CreateAsync(new IdentityRole(rolename)).GetAwaiter().GetResult();

            await _userManager.AddToRoleAsync(user, rolename);
            return true;
        }
        return false;
    }
}