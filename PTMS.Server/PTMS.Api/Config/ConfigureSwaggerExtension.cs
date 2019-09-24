using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using PTMS.Api.Attributes;
using PTMS.Common;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PTMS.Api.Config
{
    public static class ConfigureSwaggerExtension
    {
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "PTMS API"
                });

                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "Enter in the field below: \"Bearer {your-token}\". Get token from the /account/login",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey",
                });

                c.OperationFilter<SwaggerAuthorizeCheckOperationFilter>();
            });
        }
    }

    internal class SwaggerAuthorizeCheckOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            // Check for authorize attribute
            context.ApiDescription.TryGetMethodInfo(out var methodInfo);

            if (methodInfo == null)
                return;

            PtmsAuthorize ptmsAttribute = null;

            if (methodInfo.MemberType == MemberTypes.Method)
            {
                ptmsAttribute = methodInfo.GetCustomAttributes(true).OfType<PtmsAuthorize>().LastOrDefault()
                    ?? methodInfo.DeclaringType.GetCustomAttributes(true).OfType<PtmsAuthorize>().LastOrDefault();

                if (ptmsAttribute != null)
                {
                    var allowAnonymous = methodInfo.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>().Any();
                    if (allowAnonymous)
                    {
                        ptmsAttribute = null;
                    }
                }
            }

            if (ptmsAttribute != null)
            {
                operation.Responses.Add(StatusCodes.Status401Unauthorized.ToString(), new Response { Description = "Unauthorized" });
                operation.Responses.Add(StatusCodes.Status403Forbidden.ToString(), new Response { Description = "Forbidden" });

                operation.Security = new List<IDictionary<string, IEnumerable<string>>>
                    {
                        new Dictionary<string, IEnumerable<string>>
                        {
                            { "Bearer", new string[] {} }
                        }
                    };

                var rolesList = (ptmsAttribute.Roles != null && ptmsAttribute.Roles.Any()) ? string.Join(", ", ptmsAttribute.Roles) : "Любая";
                operation.Summary += $" Роль: [{rolesList}]";
            }
            else
            {
                operation.Summary += " Доступно анонимно";
            }
        }
    }
}
