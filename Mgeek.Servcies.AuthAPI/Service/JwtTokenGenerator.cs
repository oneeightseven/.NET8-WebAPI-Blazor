namespace Mgeek.Servcies.AuthAPI.Service;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtOptions _jwtOptions;
    public JwtTokenGenerator(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }
    public string GenerateToken(ApplicationUser applicationUser, IEnumerable<string> roles)
    {
        var key = Encoding.ASCII.GetBytes(_jwtOptions.Secret!);
        var claims = new List<Claim>
        {
            new (JwtRegisteredClaimNames.Email, applicationUser.Email!),
            new (JwtRegisteredClaimNames.Sub, applicationUser.Id!),
            new (JwtRegisteredClaimNames.Name, applicationUser.Name!),
        };
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role,role)));
        
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Audience = _jwtOptions.Audience,
            Issuer = _jwtOptions.Issuer,
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}