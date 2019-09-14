using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;

namespace Swashbuckle.AspNetCore.Swagger
{
    public static class SwaggerExtensions
    {
        public static void IncludeXmlComments(this SwaggerGenOptions swaggerGenOptions, params Type[] types)
        {
            for (int i = 0; i < types.Length; i++)
            {
                var xmlFile = $"{types[i].Assembly.GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                swaggerGenOptions.IncludeXmlComments(xmlPath);
            }
        }
    }
}