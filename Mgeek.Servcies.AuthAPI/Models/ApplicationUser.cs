namespace Mgeek.Servcies.AuthAPI.Models;

public class ApplicationUser : IdentityUser
{
    public string? Name { get; set; }
}