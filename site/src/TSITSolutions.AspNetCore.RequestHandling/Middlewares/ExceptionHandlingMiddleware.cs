using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace TSITSolutions.ContactSite.RequestHandlingCore.Middlewares;

internal class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) => _logger = logger;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
#pragma warning disable CA1031 // Do not catch general exception types
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            if (await TryHandleExceptionAsync(context, e))
            {
                _logger.LogWarning(e, "Handled exception");
            }
            else
            {
                _logger.LogError(e, "Unhandled exception");
            }
        }
#pragma warning restore CA1031 // Do not catch general exception types
    }

    private static async Task<bool> TryHandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        var (handled, statusCode) = GetStatusCode(exception);
        var response = new
        {
            Title = GetTitle(exception),
            Status = statusCode,
            Detail = exception.Message,
            Errors = GetErrors(exception)
        };
        httpContext.Response.ContentType = "application/problem+json";
        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
        return handled;
    }
    private static (bool handled, int statusCode) GetStatusCode(Exception exception) =>
        exception switch
        {
            ValidationException => (true, StatusCodes.Status422UnprocessableEntity),
            BadHttpRequestException => (true, StatusCodes.Status400BadRequest),
            _ => (false, StatusCodes.Status500InternalServerError)
        };
    private static string GetTitle(Exception exception) =>
        exception switch
        {
            ApplicationException applicationException => applicationException.Message,
            _ => "Server Error"
        };
    private static Dictionary<string, string> GetErrors(Exception exception)
    {
        return exception switch
        {
            ValidationException validationException => validationException.Errors.ToDictionary(x => x.PropertyName, x => x.ErrorMessage),
            _ => new Dictionary<string, string>()
        };
    }
}