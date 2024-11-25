using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Web_API.Middlewares
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private const string ApiKeyHeaderName = "X-Api-Key";

        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IConfiguration configuration)
        {
            var apiKey = configuration.GetValue<string>("ApiKey");

            if (!context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var providedApiKey) ||
                !apiKey.Equals(providedApiKey))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Chave de API inválida.");
                return;
            }

            await _next(context);
        }
    }
}
