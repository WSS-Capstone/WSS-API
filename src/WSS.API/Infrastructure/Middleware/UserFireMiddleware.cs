using Microsoft.AspNetCore.Authorization;

namespace WSS.API.Infrastructure.Middleware;

public class UserFireMiddleware
{
    private readonly RequestDelegate _next;

    public UserFireMiddleware(RequestDelegate next)
    {
        this._next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {

        await this._next(httpContext);

    }
}