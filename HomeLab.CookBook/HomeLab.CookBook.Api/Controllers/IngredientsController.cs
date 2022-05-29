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
    /// Ingredients controller
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class IngredientsController : ControllerBase
    {
        private readonly ILogger<IngredientsController> _logger;
        private readonly IIngredientsService _service;
        private readonly IMapper _mapper;

        /// <summary>
        /// Ingredient controller constructor.
        /// </summary>
        /// <param name="logger">Logger component</param>
        /// <param name="service">Service</param>
        /// <param name="mapper">Mapper component</param>
        public IngredientsController(ILogger<IngredientsController> logger, IIngredientsService service, IMapper mapper)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
        }

        /// <summary>
        /// Request to create an ingredient for a specific Recipe Step
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /ingredients
        ///     {
        ///         "stepId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///         "name": "Parsley",
        ///         "details": "Cut before use.",
        ///         "amount": 20,
        ///         "amountType": "g"
        ///     }
        ///
        /// </remarks>
        /// <param name="model">Ingredient model</param>
        /// <returns>Ingredient overview</returns>
        /// <response code="201">Ingredient created.</response>
        /// <response code="404">Step reference not found.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(IngredientOverviewModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Create(IngredientCreateModel model)
        {
            var step = _mapper.Map<IngredientModel>(model);
            try
            {
                var result = await _service.AddAsync(step);
                return Created(nameof(Create), _mapper.Map<IngredientOverviewModel>(result));

            }
            catch (NotFoundException)
            {
                return NotFound();
            }

        }

        /// <summary>
        /// Retrieves a list of ingredients.
        /// </summary>
        /// <returns>List of ingredients</returns>
        /// <response code="200">Ingredient list retrieved.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IngredientOverviewModel[]))]
        public IActionResult GetAll()
        {
            var result = _service.Get().ToList();

            return Ok(_mapper.Map<IEnumerable<IngredientOverviewModel>>(result));
        }

        /// <summary>
        /// Retrieve an ingredient from its ID
        /// </summary>
        /// <param name="id">Guid reference of the ingredient</param>
        /// <returns>Ingredient data</returns>
        /// <response code="200">Ingredient structure retrieved.</response>
        /// <response code="404">Ingredient not found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IngredientDetailsModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> Get(Guid id)
        {
            var recipe = await _service.GetByIdAsync(id);
            var result = _mapper.Map<IngredientDetailsModel>(recipe);
            return Ok(result);
        }

        /// <summary>
        /// Patch operation for the ingredient structure.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PATCH /ingredients/{id}
        ///     [{
        ///         "path": "/name",
        ///         "op": "replace",
        ///         "value": "Salt"
        ///     }]
        ///
        /// </remarks>
        /// <param name="id">Ingredient reference Id.</param>
        /// <param name="patch">Patch object</param>
        /// <returns>Returns updated Ingredient.</returns>
        /// <response code="200">Ingredient structure updated.</response>
        /// <response code="404">Ingredient not found.</response>
        /// <response code="400">Operation not permitted.</response>
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IngredientOverviewModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Patch(Guid id, [FromBody] JsonPatchDocument<IngredientModel> patch)
        {
            var validOperations = new List<(string Operation, string Path)>()
            {
                ("replace", "/name"),
                ("replace", "/details"),
                ("replace", "/amount"),
                ("replace", "/amountType"),
            };

            if (!patch.Operations.TrueForAll(x => validOperations.Contains((x.op, x.path))))
            {
                return BadRequest("Operation not permitted.");
            }

            try
            {
                var model = new IngredientModel() { Id = id };
                patch.ApplyTo(model);
                var result = await _service.UpdateAsync(model);

                return Ok(_mapper.Map<IngredientOverviewModel>(result));
            }
            catch (NotFoundException)
            {
                return NotFound();
            }

        }

        /// <summary>
        /// Deletes an ingredient using its reference.
        /// </summary>
        /// <param name="id">Ingredient reference Id.</param>
        /// <returns>HTTP OK</returns>
        /// <response code="200">Ingredient was deleted.</response>
        /// <response code="404">Ingredient not found.</response>
        [HttpDelete("{id}")]
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
