using Microsoft.AspNetCore.Mvc;

namespace Mgeek.Services.Orchestrator.Controllers;

[Route("api/BaseController")]
[ApiController]
public class BaseController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok();
    }
}