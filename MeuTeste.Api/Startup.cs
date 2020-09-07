using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.SwaggerGen;
using MeuTeste.Ioc;
using MeuTeste.CrossCutting;
using Polly.Timeout;
using Polly.Extensions.Http;
using System.Net;
using Polly;

namespace MeuTeste.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient(ServiceClientNames.ResourceService, c =>
            {
                //c.BaseAddress = new Uri("http://localhost:6000");
                //c.BaseAddress = new Uri("http://localhost/teste/WeatherForecast
                c.BaseAddress = new Uri("http://localhost");
                c.DefaultRequestHeaders.Add("Accept", "application/json");
                c.DefaultRequestHeaders.Add("User-Agent", "Sample-Application");
            })
                .AddPolicyHandler(p =>
                HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
                    .Or<TimeoutRejectedException>()
                    .WaitAndRetryAsync(10, attempt =>
                    {
                        Console.WriteLine(attempt);
                        //Tell the service about attempt number
                        p.Headers.Remove("X-Retry");
                        p.Headers.Add("X-Retry", attempt.ToString());

                        //Set the wait interval
                        return TimeSpan.FromSeconds(attempt * 3);

                    })
                );
            services.AddPollyPolices(Configuration);
            services.AddControllers();
            services.AddSwagger();
            services.AddSwaggerGen();
            services.AddHandlers();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger()
                .UseSwaggerUI(c => c.SwaggerEndpoint("v1/swagger.json", "MeuTeste.Api"));
        }
    }
}
