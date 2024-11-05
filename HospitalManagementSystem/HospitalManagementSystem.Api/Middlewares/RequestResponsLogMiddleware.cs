using System.Security.Claims;

namespace HospitalManagementSystem.Api.Middlewares
{
    public class RequestResponsLogMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponsLogMiddleware> _logger;

        public RequestResponsLogMiddleware(RequestDelegate next,
        ILogger<RequestResponsLogMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            LogRequest(context);

            var originalBodyStream = context.Response.Body;
            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                await _next(context);

                context.Response.Body.Seek(0, SeekOrigin.Begin);
                var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
                context.Response.Body.Seek(0, SeekOrigin.Begin);

                LogResponse(context);

                await responseBody.CopyToAsync(originalBodyStream);
            }
        }

        private void LogRequest(HttpContext context)
        {
            var request = context.Request;

            var requestLog = $"Outgoing Request: HTTP {request.Method} {request.Path} Host {request.Host}";

            _logger.LogInformation(requestLog);
        }

        private void LogResponse(HttpContext context)
        {
            var response = context.Response;

            var responseLog = $"Outgoing Response: HTTP {response.StatusCode}, UserId {context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value}";
            _logger.LogInformation(responseLog);
        }
    }
}