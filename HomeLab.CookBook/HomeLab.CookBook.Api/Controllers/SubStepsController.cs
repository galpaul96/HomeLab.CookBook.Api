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
    /// SubSteps controller.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class SubStepsController : ControllerBase
    {
        private readonly ILogger<SubStepsController> _logger;
        private readonly ISubStepsService _service;
        private readonly IMapper _mapper;

        /// <summary>
        /// SubSteps controller constructor
        /// </summary>
        /// <param name="logger">Logger component</param>
        /// <param name="service">Service</param>
        /// <param name="mapper">Mapper component</param>
        public SubStepsController(ILogger<SubStepsController> logger, ISubStepsService service, IMapper mapper)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
        }

        /// <summary>
        /// Request to create a SubSteps of a Step
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /substeps
        ///     {
        ///         "description": "Spice it up.",
        ///         "duration": "00:20:00",
        ///         "stepId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
        ///     }
        ///
        /// </remarks>
        /// <param name="model">SubSteps model</param>
        /// <returns>SubSteps overview</returns>
        /// <response code="201">SubSteps created.</response>
        /// <response code="404">Step reference not found.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(SubStepOverviewModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Create(SubStepCreateModel model)
        {
            var step = _mapper.Map<SubStepModel>(model);

            var result = await _service.AddAsync(step);

            return Ok(_mapper.Map<SubStepOverviewModel>(result));
        }

        /// <summary>
        /// Retrieves a list of SubSteps.
        /// </summary>
        /// <returns>List of SubSteps.</returns>
        /// <response code="200">SubSteps list retrieved.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SubStepOverviewModel[]))]
        public IActionResult GetAll()
        {
            var result = _service.Get().ToList();

            return Ok(_mapper.Map<IEnumerable<SubStepOverviewModel>>(result));
        }

        /// <summary>
        /// Retrieve a SubStep from its reference ID
        /// </summary>
        /// <param name="id">Guid reference of the SubSteps</param>
        /// <returns>Recipe step data</returns>
        /// <response code="200">SubSteps structure retrieved.</response>
        /// <response code="404">SubSteps not found.</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SubStepDetailsModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(Guid id)
        {
            var recipe = await _service.GetByIdAsync(id);
            var result = _mapper.Map<SubStepDetailsModel>(recipe);
            return Ok(result);
        }

        /// <summary>
        /// Patch operation for the SubSteps structure.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PATCH /substeps/{id}
        ///     [{
        ///         "path": "/title",
        ///         "op": "replace",
        ///         "value": "Let it sit for 30 mins."
        ///     }]
        ///
        /// </remarks>
        /// <param name="id">SubSteps reference Id.</param>
        /// <param name="patch">Patch object</param>
        /// <returns>Returns updated SubSteps.</returns>
        /// <response code="200">SubSteps structure updated.</response>
        /// <response code="404">SubSteps not found.</response>
        /// <response code="400">Operation not permitted.</response>
        [HttpPatch("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SubStepOverviewModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Patch(Guid id, [FromBody] JsonPatchDocument<SubStepModel> patch)
        {
            var validOperations = new List<(string Operation, string Path)>()
            {
                ("replace", "/title"),
                ("replace", "/description")
            };

            if (!patch.Operations.TrueForAll(x => validOperations.Contains((x.op, x.path))))
            {
                return BadRequest("Operation not permitted.");
            }

            try
            {
                var model = new SubStepModel() { Id = id };
                patch.ApplyTo(model);
                var result = await _service.UpdateAsync(model);

                return Ok(_mapper.Map<SubStepOverviewModel>(result));
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Deletes a SubStep using its reference.
        /// </summary>
        /// <param name="id">SubStep reference Id.</param>
        /// <returns>HTTP OK</returns>
        /// <response code="200">SubStep was deleted.</response>
        /// <response code="404">SubStep not found.</response>
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
