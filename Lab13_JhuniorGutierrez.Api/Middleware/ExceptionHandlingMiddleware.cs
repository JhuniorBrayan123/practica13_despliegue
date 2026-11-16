using Lab13_JhuniorGutierrez.Application.Common.Exceptions;
using Lab13_JhuniorGutierrez.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab13_JhuniorGutierrez.Api.Middleware;


public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            var (status, title) = ex switch
            {
                NotFoundException => (StatusCodes.Status404NotFound, "Not found"),
                ConflictException => (StatusCodes.Status409Conflict, "Conflict"),
                UnauthorizedException => (StatusCodes.Status401Unauthorized, "Unauthorized"),
                ForbiddenException => (StatusCodes.Status403Forbidden, "Forbidden"),
                DomainException => (StatusCodes.Status400BadRequest, "Business rule violated"),
                DbUpdateException => (StatusCodes.Status409Conflict, "Database constraint violated"),
                _ => (StatusCodes.Status500InternalServerError, "Unexpected error")
            };

            if (status == StatusCodes.Status500InternalServerError)
                logger.LogError(ex, "Unhandled exception");

            var detail = ex switch
            {
                DbUpdateException => "The operation conflicts with related data.",
                _ when status == StatusCodes.Status500InternalServerError => "An unexpected error occurred.",
                _ => ex.Message
            };

            context.Response.StatusCode = status;
            await context.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status = status,
                Title = title,
                Detail = detail,
                Instance = context.Request.Path
            });
        }
    }
}
