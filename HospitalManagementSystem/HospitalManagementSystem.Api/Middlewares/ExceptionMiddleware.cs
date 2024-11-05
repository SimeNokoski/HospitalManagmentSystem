using HospitalManagementSystem.Shared;
using System.Net;
using System.Net.Mime;
using System.Security.Claims;
using System.Text.Json;

namespace HospitalManagementSystem.Api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = MediaTypeNames.Application.Json;

            switch (exception)
            {
                case AppointmentNotFoundException:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;

                case DoctorNotFoundException:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;

                case MedicalRecordNotFoundException:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;

                case InvalidDateException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;

                case PatientNotFoundException:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;

                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = exception.Message
            };

            var jsonResponse = JsonSerializer.Serialize(response);
            _logger.LogError("Response: {jsonResponse} UserId {userid}", jsonResponse, context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await context.Response.WriteAsync(jsonResponse);
        }
    }
}