using System.Net;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace HQPlus.Task3.Api.Infrastructure {
    public sealed class HttpGlobalExceptionHandler : IExceptionFilter {

        private readonly ILogger<HttpGlobalExceptionHandler> _logger;

        public HttpGlobalExceptionHandler(ILogger<HttpGlobalExceptionHandler> logger) {
            _logger = logger;
        }

        public void OnException(ExceptionContext context) {
            _logger.LogError(new EventId(context.Exception.HResult),
                context.Exception,
                context.Exception.Message);

            var errorResponse = new ErrorResponse {
                Messages = new[] {"An unexpected exception occurred"},
                AdditionalDetail = context.Exception
            };

            context.Result = new InternalServerErrorObjectResult(errorResponse);
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.ExceptionHandled = true;
        }

        private class ErrorResponse {

            public string[] Messages { get; set; }

            public object AdditionalDetail { get; set; }

        }
    }
}