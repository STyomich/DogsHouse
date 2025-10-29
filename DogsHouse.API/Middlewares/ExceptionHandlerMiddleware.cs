using System.Text.Json;
using DogsHouse.Application.Exceptions;

namespace DogsHouse.API.Middlewares;

public class ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger = logger;

    private static readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (NotFoundException ex)
        {
            LogException(ex);

            await HandleExceptionAsync(context, ex, StatusCodes.Status404NotFound, "Entity not found.");
        }
        catch (OperationCanceledException ex)
        {
            LogException(ex);

            await HandleExceptionAsync(context, ex, StatusCodes.Status499ClientClosedRequest, "Request was canceled.");
        }
        catch (Exception ex)
        {
            LogException(ex);

            await HandleExceptionAsync(context, ex, StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }

    private void LogException(Exception exception)
    {
        var logMessage = $@"---- Exception Occurred ----
            Type: {exception.GetType().Name}
            Message: {exception.Message}
            Stack Trace: {exception.StackTrace}";
        _logger.LogError(logMessage);

        // Log inner exceptions recursively
        if (exception.InnerException != null)
        {
            LogInnerException(exception.InnerException);
        }
    }

    private void LogInnerException(Exception innerException)
    {
        var innerExceptionLog = $@"---- Inner Exception ----
            Type: {innerException.GetType().Name}
            Message: {innerException.Message}
            Stack Trace: {innerException.StackTrace}";
        _logger.LogError(innerExceptionLog);

        if (innerException.InnerException != null)
        {
            LogInnerException(innerException.InnerException); // Recursive logging for nested inner exceptions.
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception, int statusCode, string userFriendlyMessage)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var response = new
        {
            StatusCode = statusCode,
            ErrorMessage = userFriendlyMessage,
            ExceptionMessage = exception.Message,
            StackTrace = exception.StackTrace,
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(response, _jsonOptions));
    }
}
