namespace ApiN5.Authentication
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _requestDelegate;
        private readonly IConfiguration _configuration;

        public ApiKeyMiddleware(RequestDelegate requestDelegate, IConfiguration configuration)
        {
            _requestDelegate = requestDelegate;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var providedApiKey = context.Request.Headers[AuthConfig.ApiKeyHeader].FirstOrDefault();
            var isValid = providedApiKey != null && IsValid(providedApiKey);

            if (!isValid)
            {
                await GenerateResponse(context,401,"Invalid Api-Key");
                return;
            }

            await _requestDelegate(context);
        }

        private static async Task GenerateResponse(HttpContext context, int httpStatusCode, string message)
        {
            context.Response.StatusCode = httpStatusCode;

            await context.Response.WriteAsync(message);
        }
        private bool IsValid(string apiKeyProvided)
        {
            if (string.IsNullOrEmpty(apiKeyProvided))
            {
                return false;
            }

            var validApiKey = _configuration.GetValue<string>(AuthConfig.AuthSection);

            return string.Equals(validApiKey, apiKeyProvided, StringComparison.Ordinal);
        }
    }
}
