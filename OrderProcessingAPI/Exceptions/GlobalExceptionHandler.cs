using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace OrderProcessingAPI.Exceptions
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            int statusCode;
            string title;
            string detail;

            switch (exception)
            {
                case NotFoundException:
                    statusCode = StatusCodes.Status404NotFound;
                    title = "Resource Not Found";
                    detail = exception.Message;
                    break;

                case BusinessRuleException:
                    statusCode = StatusCodes.Status400BadRequest;
                    title = "Business Rule Validation Failed";
                    detail = exception.Message;
                    break;

                default:
                    statusCode = StatusCodes.Status500InternalServerError;
                    title = "Server Error";
                    detail = "An unexpected server error occurred.";
                    _logger.LogError(exception, "An unhandled exception occurred.");
                    break;
            }

            httpContext.Response.StatusCode = statusCode;

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = detail,
                Instance = httpContext.Request.Path
            };

            await httpContext.Response.WriteAsJsonAsync(
                problemDetails,
                cancellationToken);

            return true;
        }
    }
}
