using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace OrderApp.Api.Middleware;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "发生未处理的异常: {Message}", exception.Message);

        var problemDetails = exception switch
        {
            // 业务异常
            InvalidOperationException => new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "业务错误",
                Detail = exception.Message,
                Instance = httpContext.Request.Path
            },

            // 参数异常
            ArgumentException => new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "参数错误",
                Detail = exception.Message,
                Instance = httpContext.Request.Path
            },

            // 未找到异常
            KeyNotFoundException => new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "资源未找到",
                Detail = exception.Message,
                Instance = httpContext.Request.Path
            },

            // 默认 500
            _ => new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "服务器内部错误",
                Detail = "发生了意外错误，请稍后重试",
                Instance = httpContext.Request.Path
            }
        };

        // 添加 TraceId 方便追踪
        problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;

        httpContext.Response.StatusCode = problemDetails.Status ?? 500;
        httpContext.Response.ContentType = "application/problem+json";

        await httpContext.Response.WriteAsync(
            JsonSerializer.Serialize(problemDetails),
            cancellationToken);

        return true; // 异常已处理
    }
}
