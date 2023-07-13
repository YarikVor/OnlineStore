using Microsoft.AspNetCore.Mvc;

namespace OnlineStore.Controllers;

[ApiController]
[Route("api/v1/ping")]
public class PingController: Controller
{
    [HttpGet]
    public OkResult Ping() => Ok();
}