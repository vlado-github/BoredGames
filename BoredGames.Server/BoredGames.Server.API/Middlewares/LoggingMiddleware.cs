using BoredGames.Server.Common.Consts;
using Microsoft.IO;
using Serilog.Context;

namespace BoredGames.Server.API.Middlewares;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;
    private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;
    private readonly string[] _skipUrlRoutes = new[] { "user", "attachment/upload" };

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
        _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await LogRequest(context);
        await LogResponse(context);
    }

    private async Task LogRequest(HttpContext context)
    {
        context.Request.EnableBuffering();

        if (context.Request.Headers.Any())
        {
            var userId = context.Request.Headers[DefaultConsts.PlayerIdHeaderKey];
            LogContext.PushProperty("UserId", userId);
        }

        //skip request log for sensitive data
        var queryString = string.Empty;
        if (HasSensitiveData(context.Request.Path))
        {
            queryString = context.Request.QueryString.HasValue ? context.Request.QueryString.Value.Replace(@"\s+", " ") : string.Empty;
            _logger.LogInformation($"Http Request Information:{Environment.NewLine}" +
                                   $"Schema:{context.Request.Scheme} " +
                                   $"Host: {context.Request.Host} " +
                                   $"Path: {context.Request.Path} " +
                                   $"QueryString: {queryString} ");
        }
        else
        {
            await using var requestStream = _recyclableMemoryStreamManager.GetStream();
            await context.Request.Body.CopyToAsync(requestStream);
            queryString = context.Request.QueryString.HasValue ? context.Request.QueryString.Value.Replace(@"\s+", " ") : string.Empty;
            _logger.LogInformation($"Http Request Information:{Environment.NewLine}" +
                                   $"Schema:{context.Request.Scheme} " +
                                   $"Host: {context.Request.Host} " +
                                   $"Path: {context.Request.Path} " +
                                   $"QueryString: {queryString} " +
                                   $"Request Body: {ReadStreamInChunks(requestStream)}");
            context.Request.Body.Position = 0;
        }
    }

    private async Task LogResponse(HttpContext context)
    {
        var originalBodyStream = context.Response.Body;

        if (context.Request.Headers.Any())
        {
            var userId = context.Request.Headers[DefaultConsts.PlayerIdHeaderKey];
            LogContext.PushProperty("UserId", userId);
        }

        await _next(context);

        //skip response log for sensitive data
        var queryString = string.Empty;
        if (HasSensitiveData(context.Request.Path))
        {
            queryString = context.Request.QueryString.HasValue ? context.Request.QueryString.Value.Replace(@"\s+", " ") : string.Empty;
            _logger.LogInformation($"Http Response Information:{Environment.NewLine}" +
                               $"Schema:{context.Request.Scheme} " +
                               $"Host: {context.Request.Host} " +
                               $"Path: {context.Request.Path} " +
                               $"QueryString: {queryString} ");
        }
        else
        {
            await using var responseBody = _recyclableMemoryStreamManager.GetStream();
            context.Response.Body = responseBody;

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            queryString = context.Request.QueryString.HasValue ? context.Request.QueryString.Value.Replace(@"\s+", " ") : string.Empty;
            _logger.LogInformation($"Http Response Information:{Environment.NewLine}" +
                               $"Schema:{context.Request.Scheme} " +
                               $"Host: {context.Request.Host} " +
                               $"Path: {context.Request.Path} " +
                               $"QueryString: {queryString} " +
                               $"Response Body: {text}");
            await responseBody.CopyToAsync(originalBodyStream);
        }
    }

    private static string ReadStreamInChunks(Stream stream)
    {
        const int readChunkBufferLength = 4096;

        stream.Seek(0, SeekOrigin.Begin);

        using var textWriter = new StringWriter();
        using var reader = new StreamReader(stream);

        var readChunk = new char[readChunkBufferLength];
        int readChunkLength;

        do
        {
            readChunkLength = reader.ReadBlock(readChunk, 0, readChunkBufferLength);
            textWriter.Write(readChunk, 0, readChunkLength);
        } while (readChunkLength > 0);

        return textWriter.ToString();
    }

    private bool HasSensitiveData(PathString path)
    {
        var pathAsString = path.ToString();
        return _skipUrlRoutes.Any(r => pathAsString.Contains(r));
    }
}