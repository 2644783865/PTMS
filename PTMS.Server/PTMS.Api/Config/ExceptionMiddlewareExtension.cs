using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

public static class ExceptionMiddlewareExtension
{
    public static void ConfigureExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                var exceptionHandlerPathFeature =
                    context.Features.Get<IExceptionHandlerPathFeature>();

                var error = GetError(exceptionHandlerPathFeature?.Error);
                var statusCode = (int)GetErrorCode(exceptionHandlerPathFeature?.Error);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = statusCode;

                var response = JsonConvert.SerializeObject(error, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

                await context.Response.WriteAsync(response);
            });
        });
    }

    private static Error GetError(Exception exception)
    {
        var message = exception?.Message;

        var error = new Error()
        {
            Message =  !string.IsNullOrEmpty(message) ? message : "Произошла неизвестная ошибка. Пожалуйста, обратитесь к администратору.",
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
