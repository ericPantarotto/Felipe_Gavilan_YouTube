using ConcurrencyApi.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace ConcurrencyApi.Controllers;

[ApiController]
[Route("[controller]")]
public class CardsController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> ProcessCard([FromBody] string card)
    {
        //new Random(); => this would not work well in a multi-Thread environment 
        var randomValue = RandomGen.NextDouble();
        var approved = randomValue > 0.1;
        await Task.Delay(1000);
        Console.WriteLine($"Card {card} processed ({approved})");
        return Ok(new {Card = card, Approved = approved});

    }
}