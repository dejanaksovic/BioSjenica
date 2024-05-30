
using System.Text.Json;
using System.Text.Json.Serialization;
using bioSjenica.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace bioSjenica.Middleware {
    public class GlobalResponseExceptionMiddleware : IMiddleware
    {
      public async Task InvokeAsync(HttpContext context, RequestDelegate next)
      {
        try {
          await next(context);
        }
        catch(RequestException e) {
          context.Response.Headers.ContentType = "application/json";
          context.Response.StatusCode = e.StatusCode;
          var details = new ProblemDetails() {
            Detail = e.ErrorMessage,
          };
          await context.Response.WriteAsync(JsonSerializer.Serialize(details));
        }
      }
    }
}