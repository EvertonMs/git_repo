using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MeuTeste.CrossCutting;
using System.Text.Json;
using MeuTeste.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace MeuTeste.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces("Application/json")]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class SampleController : ControllerBase
    {

        readonly IHttpClientFactory _httpClientFactory;

        public SampleController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        //[HttpGet, Authorize]
        [HttpGet, AllowAnonymous]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        public async Task<ActionResult<String>> Get(String word)
        {
            if (!String.IsNullOrWhiteSpace(word))
            {
                var result = await _httpClientFactory
                    .CreateClient(ServiceClientNames.ResourceService)
                    //.GetAsync($"/api/value?text={word}");
                    .GetAsync($"/teste/WeatherForecast");
                if (result.IsSuccessStatusCode)
                {
                    Console.WriteLine("ok");

                    var resultJson = await result.Content.ReadAsStringAsync();
                    var teste = JsonSerializer.Deserialize<List<Objeto>>(resultJson);
                    return Ok(await result.Content.ReadAsStringAsync());
                }
            }
            Console.WriteLine("noContent");
            //return NoContent("IIS localhost/teste/weatherforecast fora");
            return NoContent();
        }
    }
}
