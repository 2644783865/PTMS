using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

public class TokenFromQueryMiddlewareExtension
{
    private readonly RequestDelegate _next;

    public TokenFromQueryMiddlewareExtension(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var hasAuthHeader = context.Request.Headers.ContainsKey("Authorization");

        if (!hasAuthHeader)
        {
            var hasTokenInQuery = context.Request.Query.TryGetValue("authJwtToken", out var jwtToken);

            if (hasTokenInQuery)
            {
                context.Request.Headers.Add("Authorization", "Bearer " + jwtToken);
            }
        }

        await _next(context);
    }
}
