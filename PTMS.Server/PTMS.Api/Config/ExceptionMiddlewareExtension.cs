﻿using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

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
                await context.Response.WriteAsync(JsonConvert.SerializeObject(error));
            });
        });
    }

    private static Error GetError(Exception exception)
    {
        var error = new Error()
        {
            Message = exception?.Message ?? "Что-то пошло не так. Обратитесь к администратору.",
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