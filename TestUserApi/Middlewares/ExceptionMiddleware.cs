namespace TestUserApi.Middlewares;
using System.Net;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> _logger)
    {
        this._next = next;
        this._logger = _logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            await HandleAsync(context, exception);
        }
    }

    public async Task HandleAsync(HttpContext context, Exception exception)
    {
        var code = HttpStatusCode.InternalServerError;
        var result = string.Empty;

        switch (exception)
        {
            case Exception ex:
                _logger.LogError(ex.ToString());
                break;
        }

        context.Response.StatusCode = (int)code;

        await context.Response.WriteAsJsonAsync(result);
    }
}