using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NDIS.Shared.Common.Models;
using NDIS.Shared.Common.Extensions;

namespace NDIS.Shared.Common.Middlewares
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred.");

                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            ApiResponse<object> response;
            int statusCode;

            switch (exception)
            {
                case ResourceNotFoundException notFoundEx:
                    statusCode = (int)HttpStatusCode.NotFound;
                    response = ApiResponse<object>.Fail(notFoundEx.Message, "404");
                    break;

                case BusinessException businessEx:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    response = ApiResponse<object>.Fail(businessEx.Message, businessEx.ErrorCode ?? "400");
                    break;

                default:
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    response = ApiResponse<object>.Fail("An unexpected error occurred.", "500");
                    break;
            }

            context.Response.StatusCode = statusCode;

            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(json);
        }
    }
}
