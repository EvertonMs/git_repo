using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;


namespace MeuTeste.Ioc
{
    public static class SwaggerExtension
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                var xmlPath = Path.Combine(AppContext.BaseDirectory, "MeuTeste.API.xml");
                options.IncludeXmlComments(xmlPath);
            });
            return services;
        }
    }
}
