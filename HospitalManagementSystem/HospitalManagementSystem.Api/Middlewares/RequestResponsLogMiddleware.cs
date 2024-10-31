using System.Net;
using System.Text;

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

                LogResponse(context, responseText); 

                await responseBody.CopyToAsync(originalBodyStream);
            }
        }

        private void LogRequest(HttpContext context)
        {
            var request = context.Request;

            var requestLog = new StringBuilder();
            requestLog.AppendLine("Incoming Request:");
            requestLog.AppendLine($"HTTP {request.Method} {request.Path}");
            requestLog.AppendLine($"Host: {request.Host}");
            requestLog.AppendLine($"Content-Type: {request.ContentType}");
            requestLog.AppendLine($"Content-Length: {request.ContentLength}");

            _logger.LogInformation(requestLog.ToString());
        }

        private void LogResponse(HttpContext context, string responseText)
        {
            var response = context.Response;

            var responseLog = new StringBuilder();
            responseLog.AppendLine("Outgoing Response:");
            responseLog.AppendLine($"HTTP {response.StatusCode}");
            responseLog.AppendLine($"Content-Type: {response.ContentType}");
            responseLog.AppendLine($"Content-Length: {response.ContentLength}");

            if (responseText.Contains("eyJ"))
            {
                responseLog.AppendLine("Response Body: [REDACTED - JWT Token]");
            }
            else
            {
                responseLog.AppendLine($"Response Body: {responseText}");
            }

            _logger.LogInformation(responseLog.ToString());

        }
    }
}
