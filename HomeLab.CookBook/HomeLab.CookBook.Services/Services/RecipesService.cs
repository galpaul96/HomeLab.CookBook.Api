using AutoMapper;
using AutoMapper.QueryableExtensions;
using HomeLab.CookBook.Domain.Entities;
using HomeLab.CookBook.Domain.Interfaces.Repositories;
using HomeLab.CookBook.Domain.Interfaces.Services;
using HomeLab.CookBook.Domain.Service;
using HomeLab.CookBook.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HomeLab.CookBook.Services.Services
{
    internal class RecipesService : IRecipesService
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<RecipesService> _logger;

        public RecipesService(IRepository repository, IMapper mapper, ILogger<RecipesService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<RecipeModel> AddAsync(RecipeModel recipe)
        {
            var result = await _repository.AddAsync(_mapper.Map<Recipe>(recipe));

            return _mapper.Map<RecipeModel>(result);

        }

        public async Task<RecipeModel> GetByIdAsync(Guid id)
        {
            var result = _repository.GetAllAsync<Recipe>();
            result.Include(x => x.Instructions).ThenInclude(x => x.Steps).ThenInclude(x=>x.Ingredients);

            var recipe = await result.ProjectTo<RecipeModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (recipe == null)
            {
                throw new NotFoundException();
            }

            return recipe;
        }

        public IQueryable<RecipeModel> Get()
        {
            var result = _repository.GetAllAsync<Recipe>();
            result.Include(x => x.Instructions).ThenInclude(x => x.Steps).ThenInclude(x=>x.Ingredients);

            return result.ProjectTo<RecipeModel>(_mapper.ConfigurationProvider);
        }

        public async Task<RecipeModel> UpdateAsync(RecipeModel recipe)
        {
            var entity = await _repository.GetByIdAsync<Recipe>(recipe.Id);
            if (entity == null)
            {
                throw new NotFoundException();
            }

            if (!string.IsNullOrWhiteSpace(recipe.Title)) entity.Title = recipe.Title;
            if (!string.IsNullOrWhiteSpace(recipe.Description)) entity.Description = recipe.Description;
            if (!string.IsNullOrWhiteSpace(recipe.Difficulty)) entity.Difficulty = recipe.Difficulty;

            await _repository.UpdateAsync(entity);

            var result = await _repository.GetByIdAsync<Recipe>(entity.Id);
            return _mapper.Map<RecipeModel>(result);
        }

        public async Task DeleteAsync(Guid id)
        {
            if (!await _repository.ExistsAsync<Recipe>(id))
            {
                throw new NotFoundException();
            }

            await _repository.DeleteAsync<Recipe>(id);
        }
    }
}
