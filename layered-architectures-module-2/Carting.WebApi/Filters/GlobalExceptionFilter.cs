using System.Net;
using Carting.BLL.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Carting.WebApi.Filters;

public class GlobalExceptionFilter : IAsyncExceptionFilter
{
    public async Task OnExceptionAsync(ExceptionContext context)
    {
        // Handle the exception here, for example, log it or return a custom error response.
        if (context.Exception is ValidationException validationException)
        {
            context.Result = new ObjectResult(new { message = "Validation failed", errors = validationException.Errors })
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            };
        }
        else if(context.Exception is NotFoundException)
        {
            context.Result = new ObjectResult(new { message = "Requested entity not found" })
            {
                StatusCode = (int)HttpStatusCode.NotFound
            };        }
        else
        {
            // Handle other exceptions, e.g., log them
            Console.WriteLine($"An unhandled exception occurred: {context.Exception.Message}");
            context.Result = new ObjectResult(new { message = "An error occurred" })
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }

        // Mark the exception as handled.
        context.ExceptionHandled = true;
    }
}