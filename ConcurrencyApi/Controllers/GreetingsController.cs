using Microsoft.AspNetCore.Mvc;
using ConcurrencyApi.Helpers;
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

     [HttpGet("onlyone/{name}")]
     public async Task<ActionResult<string>> GetGreetingOnlyOne(string name)
     {
          var waitingTime = RandomNumberGen.NextDouble() * 10 + 1;
          await Task.Delay(TimeSpan.FromSeconds(waitingTime));
          
          return $"Hello, {name}";
     }

     [HttpGet("goodbye/{name}")]
     public async Task<ActionResult<string>> GetGreetingGoodbye(string name)
     {
          var waitingTime = RandomNumberGen.NextDouble() * 10 + 1;
          await Task.Delay(TimeSpan.FromSeconds(waitingTime));
          
          return $"Goodbye, {name}";
     }


     [HttpGet("asyncvoid/{name}")]
     public async Task<ActionResult<string>> GetGreetingVoidAsync(string name)
     {
          //HACK: we expect the entire application to crash
          //await Task.Delay(1);
          // try{
          //      MyAsyncVoidMethod();
          // }
          // catch (ApplicationException){
          //     throw;
          // }
          
          await MyAsyncTaskMethod();

          //MySyncTaskMethod();

          return $"Hello, {name}";
     }
     private async void MyAsyncVoidMethod(){
          await Task.Delay(1);
          throw new ApplicationException();
     }
     private async Task MyAsyncTaskMethod(){
          await Task.Delay(1);
          throw new ApplicationException();
     }
     private void  MySyncTaskMethod(){
          throw new ApplicationException();
     }
}
