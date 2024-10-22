using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Net;

namespace TestsProd
{

    class GarbageAdressException : Exception
    {
        public GarbageAdressException()
            : base("garbage adress") { }
    }

    class FiasLevelException : Exception
    {
        public FiasLevelException()
            : base("insufficient fias_level") { }
    }

    class EmptyTokenException : Exception
    {
        public EmptyTokenException()
            : base("The config file has an empty token field") { }
    }

    public class ExceptionHandlingMiddleware: IMiddleware
    {
    
        private readonly ILogger<ExceptionHandlingMiddleware> _Logger;

        public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
        {

            _Logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch(GarbageAdressException ex)
            {
                _Logger.LogWarning(ex.Message);
                await HandleMatchExceptionAsync(context, ex);
            }
            catch (FiasLevelException ex)
            {
                _Logger.LogWarning(ex.Message);
                await HandleMatchExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                _Logger.LogError($"An error occurred while processing your request: {ex.Message}");
                await HandleExceptionAsync(context, ex);
            }

        }
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var problemDetails = new ProblemDetails
            {
                Status = (int)HttpStatusCode.InternalServerError,
                Type = exception.GetType().Name,
                Title = "An unhandled error occurred",
                Detail = "check the log file"
            };
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
        private async Task HandleMatchExceptionAsync(HttpContext context, Exception exception)
        {

            var problemDetails = new ProblemDetails
            {
                Status = 400,
                Type = exception.GetType().Name,
                Title = exception.Message,
                Detail = exception.Message
            };
            context.Response.StatusCode = 400;
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }

}
