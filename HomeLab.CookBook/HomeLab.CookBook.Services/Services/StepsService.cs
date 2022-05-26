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
    internal class StepsService : IStepsService
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<StepsService> _logger;

        public StepsService(IRepository repository, IMapper mapper, ILogger<StepsService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<StepModel> AddAsync(StepModel recipe)
        {
            if (!await _repository.ExistsAsync<Recipe>(recipe.RecipeId))
            {
                throw new NotFoundException();
            }
            var result = await _repository.AddAsync(_mapper.Map<Step>(recipe));

            return _mapper.Map<StepModel>(result);
        }

        public async Task<StepModel> GetByIdAsync(Guid id)
        {
            var result = await _repository.GetByIdAsync<Step>(id, x => x.Instruction,x=>x.Ingredients);

            if (result == null)
            {
                throw new NotFoundException();
            }

            return _mapper.Map<StepModel>(result);
        }

        public IQueryable<StepModel> Get()
        {
            var result = _repository.GetAllAsync<Step>();
            result.Include(x => x.Instruction);
            result.Include(x => x.Ingredients);

            return result.ProjectTo<StepModel>(_mapper.ConfigurationProvider);
        }

        public async Task<StepModel> UpdateAsync(StepModel step)
        {
            var entity = await _repository.GetByIdAsync<Step>(step.Id);
            if (entity == null)
            {
                throw new NotFoundException();
            }
            
            if (!string.IsNullOrWhiteSpace(step.Description)) entity.Description = step.Description;
            if (step.Duration.Ticks > 0) entity.Duration = step.Duration;

            await _repository.UpdateAsync(entity);

            var result = await _repository.GetByIdAsync<Step>(entity.Id);
            return _mapper.Map<StepModel>(result);
        }

        public async Task DeleteAsync(Guid id)
        {
            if (!await _repository.ExistsAsync<Step>(id))
            {
                throw new NotFoundException();
            }

            await _repository.DeleteAsync<Step>(id);
        }
    }
}
