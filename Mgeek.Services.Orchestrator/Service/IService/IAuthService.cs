using Mgeek.Services.Orchestrator.Models.AuthAPI;

namespace Mgeek.Services.Orchestrator.Service.IService;

public interface IAuthService
{
    Task<string?> Login();
}