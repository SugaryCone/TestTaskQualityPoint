using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace TestsProd
{
    using Microsoft.AspNetCore.Diagnostics;
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<CleanAddressController> _Logger;

        public GlobalExceptionHandler(ILogger<CleanAddressController> logger)
        {
            _Logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
    HttpContext httpContext,
    Exception exception,
    CancellationToken cancellationToken)
        {
            _Logger.LogError(
                $"An error occurred while processing your request: {exception.Message}");

            var problemDetails = new ProblemDetails
            {
                Status = (int)HttpStatusCode.InternalServerError,
                Type = exception.GetType().Name,
                Title = "An unhandled error occurred",
                Detail = exception.Message
            };

            await httpContext
                .Response
                .WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}
