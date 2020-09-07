using MediatR;
using MeuTeste.Application.Command;
using MeuTeste.Application.Response;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text.Json;
using MeuTeste.Application.Response;
using System.Collections.Generic;

namespace MeuTeste.Application.Handlers
{
    public class SomaHandle : IRequestHandler<SomaCommand, SomaResponse>
    {

        private readonly HttpClient _httpClient;
        private readonly string _remoteServiceBaseUrl;


        public SomaHandle(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        /*
        private Task<string> methodAsync1()
        {
            return Task.Delay(10000)
                .ContinueWith(t => "Hello");
        }

        private Task<string> methodAsync2()
        {
            return Task.Run(() =>
            {
                
                return "Hello";
            });
        }

        private Task<string> methodAsync3()
        {
            return Task.Run(() =>
            {
                return "qualquer coisa";
            });
        }


        async ValueTask MethodWithoutReturnValue()
        {
            await Task.Delay(1);
        }

        async ValueTask<int> MethodReturningValue()
        {
            await Task.Delay(1);
            return 1;
        }
        public async Task<string> GetString()
        {
            System.Threading.Thread.Sleep(5000);
            return await Task.FromResult("Hello");
        }

        private Task<int> Soma(int a, int b)
        {
            return Task.FromResult( a + b);
        }
        */

        public async Task<SomaResponse> Handle(SomaCommand request, CancellationToken cancellationToken)
        {
            var teste = await RunHttpRequest();
            //var blalal = JsonSerializer.Deserialize<List<MeuTeste.Domain.Objeto>>(teste);
            //var deserializado = JsonSerializer.Serialize<WeatherForecastResponse>(teste);

            var result = new SomaResponse();
            result.resultado = request.Valor1 + request.Valor2;
            return result;
            //throw new NotImplementedException();
        }

        

        private async Task<HttpResponseMessage> RunHttpRequest()
        {
            //var result = await _httpClientFactory
            //        .CreateClient(ServiceClientNames.ResourceService)
            //        //.GetAsync($"/api/value?text={word}");
            //        .GetAsync($"/teste/WeatherForecast");
            //if (result.IsSuccessStatusCode)
            //{
            //    Console.WriteLine("ok");
            //    return Ok(await result.Content.ReadAsStringAsync());
            //}

            //var result = await _httpClientFactory

            //await Task.Delay(100); //GERA ERRO

            var result = await _httpClient
                        .GetAsync(@"http://localhost/teste/weatherforecast");
            var teste =  await result.Content.ReadAsStringAsync();

            var blalal = JsonSerializer.Deserialize<List<Este>>(teste);

            return result;

            //BaseAddress = new Uri(@"http://localhost/teste/weatherforecast"),
            //var client = new HttpClient()
            //{
            //    BaseAddress = new Uri(@"http://localhost/teste/weatherforecast"),
            //    Timeout = TimeSpan.FromSeconds(3)
            //};

            //var random = new Random();
            //var x = random.Next(100);
            //if (x % 3 == 0)
            //{
            //    throw new HttpRequestException("Divided by 3");
            //}
            //else if (x % 2 != 0)
            //{
            //    return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            //}

            
        }
    }

    public class Este
    {
        public DateTime date { get; set; }
        public int temperatureC { get; set; }
        public int temperatureF { get; set; }
        public string summary { get; set; }
    }
}
