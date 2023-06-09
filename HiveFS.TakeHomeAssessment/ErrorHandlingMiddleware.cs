using HiveFS.FruitData;
using Serilog;
using Serilog.Context;

namespace HiveFS.TakeHomeAssessment;

public class ErrorHandlingMiddleware : IMiddleware
{
    readonly ILogger<FruitRepository> _logger;

    public ErrorHandlingMiddleware(ILogger<FruitRepository> logger)
    {
        _logger = logger;
    }
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            var errorId = Guid.NewGuid().ToString();
            LogContext.PushProperty("ErrorId", errorId);
            LogContext.PushProperty("StackTrace", exception.StackTrace);

            _logger.LogError($"{exception.Message} Request failed with Status Code {context.Response.StatusCode} and Error Id {errorId}.");
            var response = context.Response;
            if (!response.HasStarted)
            {
                response.ContentType = "application/text";
                response.StatusCode = 500;
                await response.WriteAsync("Internal Server Error.");
            }
            else
            {
                Log.Warning("Can't write error response. Response has already started.");
            }
        }
    }
}
