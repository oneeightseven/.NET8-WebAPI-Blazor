using Microsoft.AspNetCore.Mvc;

namespace Mgeek.Services.Orchestrator.Controllers;

[Route("api/KekController")]
[ApiController]
public class KekController : ControllerBase
{
    [HttpGet]
    [Route("GetKek")]
    public string GetKek()
    {
        return "KEK";
    }
}