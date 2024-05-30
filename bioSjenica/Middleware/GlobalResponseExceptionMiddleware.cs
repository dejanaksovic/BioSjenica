
using bioSjenica.Exceptions;

namespace bioSjenica.Middleware {
    public class GlobalResponseExceptionMiddleware : IMiddleware
    {
      public async Task InvokeAsync(HttpContext context, RequestDelegate next)
      {
        try {
          next(context);
        }
        catch(RequestException e) {
          
        }
      }
    }
}