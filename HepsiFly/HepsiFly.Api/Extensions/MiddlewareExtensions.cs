using HepsiFly.Api.Middlewares;

namespace HepsiFly.Api.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalExceptionMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<GlobalExceptionHandlerMiddleware>();
    }
    
    public static IApplicationBuilder UseRequestAndResponseLoggingMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestAndResponseLoggingMiddleware>();
    }
}