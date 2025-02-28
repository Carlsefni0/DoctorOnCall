using System.ComponentModel.DataAnnotations;
using System.Security.Authentication;
using DoctorOnCall.Exceptions;
using DoctorOnCall.RepositoryInterfaces;

namespace DoctorOnCall.Utils;

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
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex, "Not Found Exception occurred.");
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsJsonAsync(new { Message = ex.Message });
        }
        catch (ValidationErrorsException ex)
        {
            _logger.LogWarning(ex, "Validation Errors Exception occurred.");
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new
            {
                Title = "One or more validation errors occurred.",
                Status = 400,
                Errors = ex.Errors
            });
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, "Validation Exception occurred.");
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new { Message = ex.Message });
        }
        catch (InvalidCredentialException ex)
        {
            _logger.LogWarning(ex, "Invalid Credential Exception occurred.");
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(new { Message = ex.Message });
        }
        catch (ScheduleConflictException ex)
        {
            _logger.LogWarning(ex, "Schedule Exception occurred.");
            context.Response.StatusCode = StatusCodes.Status409Conflict;
            await context.Response.WriteAsJsonAsync(new { Message = ex.Message });
        }
        catch (AuthenticationException ex)
        {
            _logger.LogWarning(ex, "Authentication Exception occurred.");
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new { Message = ex.Message });
        }
        catch (ForbiddenAccessException ex)
        {
            _logger.LogWarning(ex, "Authentication Exception occurred.");
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsJsonAsync(new { Message = ex.Message });
        }
        catch (UoWTransactionException ex)
        {
            _logger.LogError(ex, "Transaction failed. Rolling back changes.");
            
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred.");
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(new { Message = "An unexpected error occurred." });
        }
    }
}

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}
public class ValidationErrorsException : ValidationException
{
    public Dictionary<string, List<string>> Errors { get; }

    public ValidationErrorsException(Dictionary<string, List<string>> errors)
        : base("Validation failed. See Errors property for details.")
    {
        Errors = errors;
    }
    
}
public class ForbiddenAccessException : Exception
{
    public ForbiddenAccessException(string message) : base(message)
    {
    }
}
public class ScheduleConflictException : Exception
{
    public ScheduleConflictException(string message)
        : base(message)
    {
    }
}