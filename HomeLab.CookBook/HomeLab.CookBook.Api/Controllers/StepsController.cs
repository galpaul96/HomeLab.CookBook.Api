using AutoMapper;
using HomeLab.CookBook.API.Models;
using HomeLab.CookBook.Domain.Interfaces.Services;
using HomeLab.CookBook.Domain.Service;
using HomeLab.CookBook.Services.Exceptions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace HomeLab.CookBook.Api.Controllers
{
    /// <summary>
    /// Steps controller.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class StepsController : ControllerBase
    {
        private readonly ILogger<StepsController> _logger;
        private readonly IStepsService _service;
        private readonly IMapper _mapper;

        /// <summary>
        /// Steps controller constructor
        /// </summary>
        /// <param name="logger">Logger component</param>
        /// <param name="service">Service</param>
        /// <param name="mapper">Mapper component</param>
        public StepsController(ILogger<StepsController> logger, IStepsService service, IMapper mapper)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
        }

        /// <summary>
        /// Request to create a Step of a Recipe
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /steps
        ///     {
        ///         "description": "Spice it up.",
        ///         "duration": "00:20:00",
        ///         "recipeId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
        ///     }
        ///
        /// </remarks>
        /// <param name="model">Step model</param>
        /// <returns>Recipe Step overview</returns>
        /// <response code="201">Recipe Step created.</response>
        /// <response code="404">Recipe reference not found.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(StepOverviewModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Create(StepCreateModel model)
        {
            var step = _mapper.Map<StepModel>(model);

            var result = await _service.AddAsync(step);

            return Ok(_mapper.Map<StepOverviewModel>(result));
        }

        /// <summary>
        /// Retrieves a list of Recipe steps.
        /// </summary>
        /// <returns>List of Recipe steps.</returns>
        /// <response code="200">Steps list retrieved.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StepOverviewModel[]))]
        public IActionResult GetAll()
        {
            var result = _service.Get().ToList();

            return Ok(_mapper.Map<IEnumerable<StepOverviewModel>>(result));
        }

        /// <summary>
        /// Retrieve a recipe step from its reference ID
        /// </summary>
        /// <param name="id">Guid reference of the recipe step</param>
        /// <returns>Recipe step data</returns>
        /// <response code="200">Recipe step structure retrieved.</response>
        /// <response code="404">Recipe step not found.</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StepDetailsModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(Guid id)
        {
            var recipe = await _service.GetByIdAsync(id);
            var result = _mapper.Map<StepDetailsModel>(recipe);
            return Ok(result);
        }

        /// <summary>
        /// Patch operation for the recipe step structure.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PATCH /steps/{id}
        ///     [{
        ///         "path": "/duration",
        ///         "op": "replace",
        ///         "value": "00:20:00"
        ///     }]
        ///
        /// </remarks>
        /// <param name="id">Recipe step reference Id.</param>
        /// <param name="patch">Patch object</param>
        /// <returns>Returns updated Recipe step.</returns>
        /// <response code="200">Recipe step structure updated.</response>
        /// <response code="404">Recipe step not found.</response>
        /// <response code="400">Operation not permitted.</response>
        [HttpPatch("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StepOverviewModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Patch(Guid id, [FromBody] JsonPatchDocument<StepModel> patch)
        {
            var validOperations = new List<(string Operation, string Path)>()
            {
                ("replace", "/description"),
                ("replace", "/duration"),
            };

            if (!patch.Operations.TrueForAll(x => validOperations.Contains((x.op, x.path))))
            {
                return BadRequest("Operation not permitted.");
            }

            try
            {
                var model = new StepModel() { Id = id };
                patch.ApplyTo(model);
                var result = await _service.UpdateAsync(model);

                return Ok(_mapper.Map<StepOverviewModel>(result));
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Deletes an recipe step using its reference.
        /// </summary>
        /// <param name="id">Recipe step reference Id.</param>
        /// <returns>HTTP OK</returns>
        /// <response code="200">Recipe step was deleted.</response>
        /// <response code="404">Recipe step not found.</response>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _service.DeleteAsync(id);

            }
            catch (NotFoundException)
            {
                return NotFound();
            }

            return Ok();
        }

    }
}
