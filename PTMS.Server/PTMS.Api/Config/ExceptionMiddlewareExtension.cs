using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public ExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<ExceptionMiddleware>();
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            var error = GetError(exception);
            var statusCode = (int)GetErrorCode(exception);
            var request = context.Request;

            _logger.LogError(exception, $"Request Url: {request.Host}{request.Path} {request.QueryString}");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var response = JsonConvert.SerializeObject(error, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            await context.Response.WriteAsync(response);
        }
    }

    private static Error GetError(Exception exception)
    {
        var message = exception?.Message;

        var error = new Error()
        {
            Message = !string.IsNullOrEmpty(message) ? message : "Произошла неизвестная ошибка. Пожалуйста, обратитесь к администратору.",
            StackTrace = exception?.StackTrace,
            InnerException = exception?.InnerException
        };

        return error;
    }

    private static HttpStatusCode GetErrorCode(Exception exception)
    {
        if (exception == null)
        {
            return HttpStatusCode.InternalServerError;
        }

        if (exception is UnauthorizedAccessException)
        {
            return HttpStatusCode.Unauthorized;
        }

        if (exception is ArgumentException)
        {
            return HttpStatusCode.BadRequest;
        }

        if (exception is KeyNotFoundException)
        {
            return HttpStatusCode.NotFound;
        }

        return HttpStatusCode.InternalServerError;
    }
}

public class Error
{
    public string Message { get; set; }
    public string StackTrace { get; set; }
    public Exception InnerException { get; set; }
}
