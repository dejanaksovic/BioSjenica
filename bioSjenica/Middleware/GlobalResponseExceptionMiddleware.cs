
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
        catch(RequestException e) 
        {
          var problemDetails = new ProblemDetails {
            Detail = e.ErrorMessage,
          };

          if(e.ExtendedInformation is not null) problemDetails.Extensions = e.ExtendedInformation;
          context.Response.Headers.ContentType = "application/json";
          context.Response.StatusCode = (int) e.StatusCode;
          await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
        }
      }
    }
}