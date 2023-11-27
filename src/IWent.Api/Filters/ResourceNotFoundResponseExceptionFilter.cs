using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using IWent.Services.Exceptions;
using Microsoft.AspNetCore.Http;

namespace IWent.Api.Filters;

public class ResourceNotFoundResponseExceptionFilter : IActionFilter, IOrderedFilter
{
    public int Order => int.MaxValue - 10;

    public void OnActionExecuting(ActionExecutingContext context)
    {
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception is ResourceDoesNotExistException resourceDoesNotExistException)
        {
            context.Result = new ObjectResult(resourceDoesNotExistException.Message)
            {
                StatusCode = StatusCodes.Status404NotFound
            };

            context.ExceptionHandled = true;
        }
    }
}