using TestUserApi.Middlewares;

namespace TestUserApi.Middelwares;

public static class ExceotionMiddlewareExtension
{
    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder applicationBuilder)
    {
        return applicationBuilder.UseMiddleware<ExceptionMiddleware>();
    }
}