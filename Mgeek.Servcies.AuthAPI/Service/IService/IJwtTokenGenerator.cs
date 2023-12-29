using Mgeek.Servcies.AuthAPI.Models;

namespace Mgeek.Servcies.AuthAPI.Service.IService;

public interface IJwtTokenGenerator
{
    string GenerateToken(ApplicationUser applicationUser, IEnumerable<string> roles);
}