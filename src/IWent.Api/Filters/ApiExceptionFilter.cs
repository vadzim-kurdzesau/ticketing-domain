using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using IWent.Services.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace IWent.Api.Filters;

public class ApiExceptionFilter : IActionFilter, IOrderedFilter
{
    private readonly ILogger<ApiExceptionFilter> _logger;

    public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
    {
        _logger = logger;
    }

    public int Order => int.MaxValue - 10;

    public void OnActionExecuting(ActionExecutingContext context)
    {
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception == null)
        {
            return;
        }

        if (context.Exception is ResourceDoesNotExistException resourceDoesNotExistException)
        {
            context.Result = new ObjectResult(resourceDoesNotExistException.Message)
            {
                StatusCode = StatusCodes.Status404NotFound
            };

            context.ExceptionHandled = true;
        }
        else if (context.Exception is not ApiException)
        {
            _logger.LogError(context.Exception, "An internal exception was thrown.");
            context.Result = new ObjectResult("An internal error occured.")
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };

            context.ExceptionHandled = true;
        }
    }
}