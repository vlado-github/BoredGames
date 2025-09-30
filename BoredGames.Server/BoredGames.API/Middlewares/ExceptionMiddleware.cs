using BoredGames.API.Middlewares.CustomResponses;
using BoredGames.Common.Exceptions;
using BoredGames.Common.Utils;
using FluentValidation;
using FluentValidation.Results;

namespace BoredGames.API.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (UnauthorizedAccessException exception)
        {
            await HandleUnauthorizedAccessExceptionAsync(context, exception);
            _logger.LogWarning(exception, exception.Message, exception.StackTrace);
        }
        catch (InvalidActionException exception)
        {
            await HandleInvalidActionExceptionAsync(context, exception);
            _logger.LogInformation(exception, exception.Message, exception.StackTrace);
        }
        catch (ValidationException exception)
        {
            await HandleValidationExceptionAsync(context, exception);
            _logger.LogInformation(exception, exception.Message, exception.Errors.ToLogMessage());
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
            _logger.LogError(exception, exception.Message);
        }
        finally
        {
            _logger.LogInformation(
                "Request {method} {url} => {statusCode}",
                context.Request?.Method,
                context.Request?.Path.Value,
                context.Response?.StatusCode);
        }
    }

    private async Task HandleInvalidActionExceptionAsync(HttpContext context, InvalidActionException exception)
    {
        var errors = new List<ValidationFailure>
        {
            new ValidationFailure(propertyName: exception.Action, exception.Message)
        };
        var validationDetails = new CustomValidationErrorDetails(exception.Message, errors);
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = validationDetails.Status;
        await context.Response.WriteAsync(validationDetails.ToString());
    }

    private async Task HandleValidationExceptionAsync(HttpContext context, ValidationException exception)
    {
        var validationDetails = new CustomValidationErrorDetails(exception.Message, exception.Errors);
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)validationDetails.Status;
        await context.Response.WriteAsync(validationDetails.ToString());
    }

    private async Task HandleUnauthorizedAccessExceptionAsync(HttpContext context, UnauthorizedAccessException exception)
    {
        var errorDetails = new UnauthorizedErrorDetails(exception.Message);
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = errorDetails.Status;
        await context.Response.WriteAsync(errorDetails.ToString());
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        string message;
        if (!CurrentEnvironment.IsProduction())
        {
            message = exception.Message 
                      + exception.StackTrace 
                      + (exception.InnerException != null ? " ---- " + exception.InnerException + exception.InnerException.StackTrace : "");
        }
        else
        {
            message = "Internal Server Error has occured.";
        }

        var errorDetails = new ServerErrorDetails(message);
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = errorDetails.Status;
        await context.Response.WriteAsync(errorDetails.ToString());
    }
}