using SmartChargingPoC.Core.Constants;
using SmartChargingPoC.DataAccess.Exceptions;
using SmartChargingPoC.WebAPI.Models;

namespace SmartChargingPoC.WebAPI.Middlewares;

public class CustomExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public CustomExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            var response = context.Response;
            response.ContentType = ApiConstants.General.ContentType;

            var responseModel = new ErrorModel
            {
                Message = exception.Message,
                StatusCode = exception switch
                {
                    DataNotFoundException => StatusCodes.Status404NotFound,
                    _ => StatusCodes.Status500InternalServerError
                }
            };
            response.StatusCode = responseModel.StatusCode;
            
            await response.WriteAsync(responseModel.ToString());
        }
    }

}