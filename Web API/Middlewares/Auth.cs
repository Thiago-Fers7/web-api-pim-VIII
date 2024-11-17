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
            // Obtém a chave API esperada do appsettings.json
            var apiKey = configuration.GetValue<string>("ApiKey");

            // Verifica se o cabeçalho "X-Api-Key" está presente na requisição e se a chave é válida
            if (!context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var providedApiKey) ||
                !apiKey.Equals(providedApiKey))
            {
                // Se a chave não for válida, retorna 401 Unauthorized
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Chave de API inválida.");
                return;
            }

            // Se a chave for válida, permite a execução do próximo middleware
            await _next(context);
        }
    }
}
