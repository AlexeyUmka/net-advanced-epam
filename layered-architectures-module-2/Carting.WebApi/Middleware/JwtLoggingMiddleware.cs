using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

public class JwtLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<JwtLoggingMiddleware> _logger;

    public JwtLoggingMiddleware(RequestDelegate next, ILogger<JwtLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        // Check if the request has a JWT token
        if (context.Request.Headers.ContainsKey("Authorization"))
        {
            var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    var handler = new JwtSecurityTokenHandler();
                    var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

                    // Log JWT token details
                    _logger.LogInformation($"JWT Token Details: {JsonSerializer.Serialize(jsonToken)}");

                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error reading JWT token: {ex.Message}");
                }
            }
        }

        await _next(context);
    }
}