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
    internal class InstructionService : IInstructionService
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<InstructionService> _logger;

        public InstructionService(IRepository repository, IMapper mapper, ILogger<InstructionService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<InstructionModel> AddAsync(InstructionModel recipe)
        {
            if (!await _repository.ExistsAsync<Recipe>(recipe.RecipeId))
            {
                throw new NotFoundException();
            }
            var result = await _repository.AddAsync(_mapper.Map<Instruction>(recipe));

            return _mapper.Map<InstructionModel>(result);
        }

        public async Task<InstructionModel> GetByIdAsync(Guid id)
        {
            var result = await _repository.GetByIdAsync<Instruction>(id, x => x.Recipe, x => x.Steps);

            if (result == null)
            {
                throw new NotFoundException();
            }

            return _mapper.Map<InstructionModel>(result);
        }

        public IQueryable<InstructionModel> Get()
        {
            var result = _repository.GetAllAsync<Instruction>();
            result.Include(x => x.Recipe);
            result.Include(x => x.Steps).ThenInclude(x=>x.Ingredients);

            return result.ProjectTo<InstructionModel>(_mapper.ConfigurationProvider);
        }

        public async Task<InstructionModel> UpdateAsync(InstructionModel step)
        {
            var entity = await _repository.GetByIdAsync<Instruction>(step.Id);
            if (entity == null)
            {
                throw new NotFoundException();
            }

            if (!string.IsNullOrWhiteSpace(step.Description)) entity.Description = step.Description;
            if (step.Duration.Ticks > 0) entity.Duration = step.Duration;

            await _repository.UpdateAsync(entity);

            var result = await _repository.GetByIdAsync<Instruction>(entity.Id);
            return _mapper.Map<InstructionModel>(result);
        }

        public async Task DeleteAsync(Guid id)
        {
            if (!await _repository.ExistsAsync<Instruction>(id))
            {
                throw new NotFoundException();
            }

            await _repository.DeleteAsync<Instruction>(id);
        }
    }
}
