using System.Diagnostics;
using Microsoft.AspNetCore.Http.Extensions;

namespace ShoppingSystem.Microservice.Middlewares;

public class LogUrlMiddleWare(ILogger<LogUrlMiddleWare> logger, RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        
        logger.LogInformation(
            "Incoming Request | Method: {Method} | Path: {Path} | Url: {Url} | TraceId: {TraceId} | IP: {RemoteIp}",
            context.Request.Method,
            context.Request.Path,
            context.Request.GetDisplayUrl(),
            context.TraceIdentifier,
            context.Connection.RemoteIpAddress);
        
        await next(context);
        
        logger.LogInformation(
            "Completed Request | {Method} {Path} | StatusCode: {StatusCode} | Elapsed: {Elapsed} ms",
            context.Request.Method,
            context.Request.Path,
            context.Response.StatusCode,
            stopwatch.ElapsedMilliseconds);
    }
}

public static class LogUrlExtension
{
    public static IApplicationBuilder UseLogUrl(this IApplicationBuilder app)
    {
        return app.UseMiddleware<LogUrlMiddleWare>();
    }
}