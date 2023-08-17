namespace ContRev.Backend;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/[controller]")]
public class TestController : ControllerBase
{
    [HttpGet()]
    public IActionResult Hello()
    {
        return Ok("Hello World");
    }
}
