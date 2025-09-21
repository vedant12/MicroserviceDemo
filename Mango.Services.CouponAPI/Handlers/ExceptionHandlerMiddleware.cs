using Mango.Services.CouponAPI.Dtos;
using System.Net;
using System.Text.Json;

namespace Mango.Services.CouponAPI.Handlers
{
    public class ExceptionHandlerMiddleware(RequestDelegate _next, ILogger<ExceptionHandlerMiddleware> _logger)
    {
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred");

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Result = null
                };

                var json = JsonSerializer.Serialize(response);

                await context.Response.WriteAsync(json);
            }
        }
    }
}
