using HomeLab.CookBook.Domain.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Mime;

namespace HomeLab.CookBook.Api.Controllers
{
    /// <summary>
    /// HealthChecks controller.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class HealthChecksController: ControllerBase
    {
        private readonly ILogger<HealthChecksController> logger;
        private readonly Configs configs;

        public HealthChecksController(ILogger<HealthChecksController> _logger, IOptions<Configs> _configs)
        {
            logger = _logger;
            configs = _configs.Value;
        }

        /// <summary>
        /// Test
        /// </summary>
        /// <returns></returns>
        /// <response code="200"></response>
        [HttpGet("testvar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult TestGet()
        {

            return Ok(configs.TestVar);
        }
        /// <summary>
        /// Test
        /// </summary>
        /// <returns></returns>
        /// <response code="200"></response>
        [HttpGet("test")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Test()
        {

            return Ok("It works!!");
        }
    }
}
