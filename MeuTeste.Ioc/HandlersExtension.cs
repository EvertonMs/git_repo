using MeuTeste.Application.Command;
using MeuTeste.Application.Handlers;
using Microsoft.Extensions.DependencyInjection;
using MediatR;

namespace MeuTeste.Ioc
{
    public static class HandlersExtension
    {
        public static IServiceCollection AddHandlers(this IServiceCollection services)
        {
            services
                .AddMediatR(typeof(SomaHandle).Assembly);
            services
                .AddMediatR(typeof(SomaCommand).Assembly);  

            return services;
        }
    }
}
