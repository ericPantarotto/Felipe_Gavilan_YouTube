using Microsoft.AspNetCore.Mvc;

namespace ConcurrencyApi.Controllers;

[ApiController]
[Route("[controller]")]
public class GreetingsController : ControllerBase
{
       [HttpGet("{name}")]
       public ActionResult<string> GetGreeting(string name)
       {
           return $"Hello, {name}";
       }
}