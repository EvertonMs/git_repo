using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using MeuTeste.Application.Command;
using MeuTeste.Application.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MeuTeste.Api.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    [Produces("Application/json")]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class FazAlgumaCoisaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FazAlgumaCoisaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Response<SomaResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Get(int a , int b)
        {
            try
            {
                var command = new SomaCommand(a, b);
                var result = await _mediator.Send(command);

                return Ok(result);
            }
            catch (Exception erro)
            {
                throw new Exception(erro.Message);
            }
            
        }
    }
}
