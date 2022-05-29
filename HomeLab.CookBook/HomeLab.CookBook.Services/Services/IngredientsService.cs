using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    internal class IngredientsService : IIngredientsService
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<IngredientsService> _logger;

        public IngredientsService(IRepository repository, IMapper mapper, ILogger<IngredientsService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IngredientModel> AddAsync(IngredientModel model)
        {
            if (!await _repository.ExistsAsync<Step>(model.StepId))
            {
                throw new NotFoundException();
            }
            var result = await _repository.AddAsync(_mapper.Map<Ingredient>(model));

            return _mapper.Map<IngredientModel>(result);
        }

        public async Task<IngredientModel> GetByIdAsync(Guid id)
        {
            var result = await _repository.GetByIdAsync<Ingredient>(id, x => x.Step);

            if (result == null)
            {
                throw new NotFoundException();
            }

            return _mapper.Map<IngredientModel>(result);
        }

        public IQueryable<IngredientModel> Get()
        {
            var result = _repository.GetAllAsync<Ingredient>();
            result.Include(x => x.Step);

            return result.ProjectTo<IngredientModel>(_mapper.ConfigurationProvider);
        }

        public async Task<IngredientModel> UpdateAsync(IngredientModel model)
        {
            var entity = await _repository.GetByIdAsync<Ingredient>(model.Id);
            if (entity == null)
            {
                throw new NotFoundException();
            }

            if (!string.IsNullOrWhiteSpace(model.Name)) entity.Name = model.Name;
            if (!string.IsNullOrWhiteSpace(model.Details)) entity.Details = model.Details;
            if (!string.IsNullOrWhiteSpace(model.Amount)) entity.Amount = model.Amount;
            if (!string.IsNullOrWhiteSpace(model.AmountType)) entity.AmountType = model.AmountType;

            await _repository.UpdateAsync(entity);

            var result = await _repository.GetByIdAsync<Ingredient>(entity.Id);
            return _mapper.Map<IngredientModel>(result);
        }

        public async Task DeleteAsync(Guid id)
        {
            if (!await _repository.ExistsAsync<Ingredient>(id))
            {
                throw new NotFoundException();
            }

            await _repository.DeleteAsync<Ingredient>(id);
        }
    }
}
