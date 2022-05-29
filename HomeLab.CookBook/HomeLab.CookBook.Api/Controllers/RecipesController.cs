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
    /// Recipes controller.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class RecipesController : ControllerBase
    {
        private readonly ILogger<RecipesController> _logger;
        private readonly IRecipesService _service;
        private readonly IMapper _mapper;

        /// <summary>
        /// Recipes controller constructor
        /// </summary>
        /// <param name="logger">Logger component</param>
        /// <param name="service">Service</param>
        /// <param name="mapper">Mapper component</param>
        public RecipesController(ILogger<RecipesController> logger, IRecipesService service, IMapper mapper)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
        }

        /// <summary>
        /// Request to create a Recipe
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /recipes
        ///     {
        ///         "title": "Soup",
        ///         "description": "Vegetable soup",
        ///         "difficulty": "Easy"
        ///     }
        ///
        /// </remarks>
        /// <param name="model">Recipe model</param>
        /// <returns>Recipe overview</returns>
        /// <response code="201">Recipe created.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(RecipeOverviewModel))]
        public async Task<IActionResult> Create(RecipeCreateModel model)
        {
            var recipe = _mapper.Map<RecipeModel>(model);

            var result = await _service.AddAsync(recipe);

            return Ok(result);
        }

        /// <summary>
        /// Retrieves a list of Recipes.
        /// </summary>
        /// <returns>List of Recipes.</returns>
        /// <response code="200">Recipe list retrieved.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RecipeOverviewModel[]))]
        public IActionResult GetAll()
        {
            var result = _service.Get();

            return Ok(_mapper.Map<IEnumerable<RecipeOverviewModel>>(result));
        }

        /// <summary>
        /// Retrieve a recipe from its reference ID
        /// </summary>
        /// <param name="id">Guid reference of the recipe</param>
        /// <returns>Recipe data</returns>
        /// <response code="200">Recipe structure retrieved.</response>
        /// <response code="404">Recipe not found.</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RecipeDetailsModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var recipe = await _service.GetByIdAsync(id);

                return Ok(_mapper.Map<RecipeDetailsModel>(recipe));
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Patch operation for the recipe structure.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PATCH /recipe/{id}
        ///     [{
        ///         "path": "/difficulty",
        ///         "op": "replace",
        ///         "value": "Medium"
        ///     }]
        ///
        /// </remarks>
        /// <param name="id">Recipe reference Id.</param>
        /// <param name="patch">Patch object</param>
        /// <returns>Returns updated Recipe.</returns>
        /// <response code="200">Recipe structure updated.</response>
        /// <response code="404">Recipe not found.</response>
        /// <response code="400">Operation not permitted.</response>
        [HttpPatch("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RecipeOverviewModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Patch(Guid id, [FromBody] JsonPatchDocument<RecipeModel> patch)
        {
            var validOperations = new List<(string Operation, string Path)>()
            {
                ("replace", "/title"),
                ("replace", "/description"),
                ("replace", "/difficulty"),
            };

            if (!patch.Operations.TrueForAll(x => validOperations.Contains((x.op, x.path))))
            {
                return BadRequest("Operation not permitted.");
            }

            try
            {
                var recipe = new RecipeModel() { Id = id };
                patch.ApplyTo(recipe);
                var result = await _service.UpdateAsync(recipe);

                return Ok(_mapper.Map<RecipeOverviewModel>(result));
            }
            catch (NotFoundException)
            {
                return NotFound();
            }

        }

        /// <summary>
        /// Deletes an recipe using its reference.
        /// </summary>
        /// <param name="id">Recipe reference Id.</param>
        /// <returns>HTTP OK</returns>
        /// <response code="200">Recipe was deleted.</response>
        /// <response code="404">Recipe not found.</response>
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
