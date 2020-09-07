using MeuTeste.CrossCutting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;

namespace MeuTeste.Ioc
{
    public static class ConfigurationExtension
    {
        public static void AddConfiguration(this IServiceCollection services)
        {
            services.AddHttpClient(ServiceClientNames.ResourceService, c =>
            {
                c.BaseAddress = new Uri("http://localhost:6000");
                c.DefaultRequestHeaders.Add("Accept", "application/json");
                c.DefaultRequestHeaders.Add("User-Agent", "Sample-Application");
            })
                .AddPolicyHandler(p =>
                HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
                    .Or<TimeoutRejectedException>()
                    .WaitAndRetryAsync(3, attempt =>
                    {

                        //Tell the service about attempt number
                        p.Headers.Remove("X-Retry");
                        p.Headers.Add("X-Retry", attempt.ToString());

                        //Set the wait interval
                        return TimeSpan.FromSeconds(attempt * 3);

                    })
                );
        }
    }
}
