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
    /// Instructions controller.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class InstructionsController : ControllerBase
    {
        private readonly ILogger<InstructionsController> _logger;
        private readonly IInstructionService _service;
        private readonly IMapper _mapper;

        /// <summary>
        /// Instructions controller constructor
        /// </summary>
        /// <param name="logger">Logger component</param>
        /// <param name="service">Service</param>
        /// <param name="mapper">Mapper component</param>
        public InstructionsController(ILogger<InstructionsController> logger, IInstructionService service, IMapper mapper)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
        }

        /// <summary>
        /// Request to create a Instruction of a Recipe
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /instructions
        ///     {
        ///         "description": "Spice it up.",
        ///         "duration": "00:20:00",
        ///         "recipeId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
        ///     }
        ///
        /// </remarks>
        /// <param name="model">Instruction model</param>
        /// <returns>Recipe Instruction overview</returns>
        /// <response code="201">Recipe Instruction created.</response>
        /// <response code="404">Recipe reference not found.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(InstructionOverviewModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Create(InstructionCreateModel model)
        {
            var mappedModel = _mapper.Map<InstructionModel>(model);

            var result = await _service.AddAsync(mappedModel);

            return Ok(_mapper.Map<InstructionOverviewModel>(result));
        }

        /// <summary>
        /// Retrieves a list of Recipe Instructions.
        /// </summary>
        /// <returns>List of Recipe Instructions.</returns>
        /// <response code="200">Instructions list retrieved.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InstructionOverviewModel[]))]
        public IActionResult GetAll()
        {
            var result = _service.Get().ToList();

            return Ok(_mapper.Map<IEnumerable<InstructionOverviewModel>>(result));
        }

        /// <summary>
        /// Retrieve a recipe Instruction from its reference ID
        /// </summary>
        /// <param name="id">Guid reference of the recipe Instruction</param>
        /// <returns>Recipe Instruction data</returns>
        /// <response code="200">Recipe Instruction structure retrieved.</response>
        /// <response code="404">Recipe Instruction not found.</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InstructionDetailsModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(Guid id)
        {
            var recipe = await _service.GetByIdAsync(id);
            var result = _mapper.Map<InstructionDetailsModel>(recipe);
            return Ok(result);
        }

        /// <summary>
        /// Patch operation for the recipe Instruction structure.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PATCH /instructions/{id}
        ///     [{
        ///         "path": "/duration",
        ///         "op": "replace",
        ///         "value": "00:20:00"
        ///     }]
        ///
        /// </remarks>
        /// <param name="id">Recipe Instruction reference Id.</param>
        /// <param name="patch">Patch object</param>
        /// <returns>Returns updated Recipe Instruction.</returns>
        /// <response code="200">Recipe Instruction structure updated.</response>
        /// <response code="404">Recipe Instruction not found.</response>
        /// <response code="400">Operation not permitted.</response>
        [HttpPatch("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InstructionOverviewModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Patch(Guid id, [FromBody] JsonPatchDocument<InstructionModel> patch)
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
                var model = new InstructionModel() { Id = id };
                patch.ApplyTo(model);
                var result = await _service.UpdateAsync(model);

                return Ok(_mapper.Map<InstructionOverviewModel>(result));
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Deletes an recipe Instruction using its reference.
        /// </summary>
        /// <param name="id">Recipe Instruction reference Id.</param>
        /// <returns>HTTP OK</returns>
        /// <response code="200">Recipe Instruction was deleted.</response>
        /// <response code="404">Recipe Instruction not found.</response>
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
