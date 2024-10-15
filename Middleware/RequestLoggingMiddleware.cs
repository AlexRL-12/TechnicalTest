using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Threading.Tasks;

namespace TechnicalTest.Middleware
{
  public class RequestLoggingMiddleware
  {
    private readonly RequestDelegate _next;

    public RequestLoggingMiddleware(RequestDelegate next)
    {
      _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
      var stopwatch = new Stopwatch();
      stopwatch.Start();

      await _next(context);

      stopwatch.Stop();
      var logMessage = $"[{context.Request.Method}] {context.Request.Path} responded in {stopwatch.ElapsedMilliseconds} ms";
      Console.WriteLine(logMessage);  
    }
  }
}
