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

       [HttpGet("async/{name}")]
       public async Task<ActionResult<string>> GetGreetingAsync(string name)
       {
            Console.WriteLine($"Thread bf the await: {Thread.CurrentThread.ManagedThreadId}");
            string text = $"starting thread: {Thread.CurrentThread.ManagedThreadId}";
            
            await Task.Delay(TimeSpan.FromSeconds(1));

            Console.WriteLine($"Thread after the await: {Thread.CurrentThread.ManagedThreadId}");
            text = $"{text} , continuing thread: {Thread.CurrentThread.ManagedThreadId}";
           return $"Hello, {name} - {text}";
       }
}