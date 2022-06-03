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
    internal class SubStepsService : ISubStepsService
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<SubStepsService> _logger;

        public SubStepsService(IRepository repository, IMapper mapper, ILogger<SubStepsService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<SubStepModel> AddAsync(SubStepModel model)
        {
            if (!await _repository.ExistsAsync<Step>(model.StepId))
            {
                throw new NotFoundException();
            }
            var result = await _repository.AddAsync(_mapper.Map<SubStep>(model));

            return _mapper.Map<SubStepModel>(result);
        }

        public async Task<SubStepModel> GetByIdAsync(Guid id)
        {
            var result = await _repository.GetByIdAsync<SubStep>(id, x => x.Step, x => x.Ingredients);

            if (result == null)
            {
                throw new NotFoundException();
            }

            return _mapper.Map<SubStepModel>(result);
        }

        public IQueryable<SubStepModel> Get()
        {
            var result = _repository.GetAllAsync<SubStep>();
            result.Include(x => x.Step);
            result.Include(x => x.Ingredients);

            return result.ProjectTo<SubStepModel>(_mapper.ConfigurationProvider);
        }

        public async Task<SubStepModel> UpdateAsync(SubStepModel step)
        {
            var entity = await _repository.GetByIdAsync<SubStep>(step.Id);
            if (entity == null)
            {
                throw new NotFoundException();
            }

            if (!string.IsNullOrWhiteSpace(step.Description)) entity.Description = step.Description;
            if (step.Duration.Ticks > 0) entity.Duration = step.Duration;

            await _repository.UpdateAsync(entity);

            var result = await _repository.GetByIdAsync<SubStep>(entity.Id);
            return _mapper.Map<SubStepModel>(result);
        }

        public async Task DeleteAsync(Guid id)
        {
            if (!await _repository.ExistsAsync<SubStep>(id))
            {
                throw new NotFoundException();
            }

            await _repository.DeleteAsync<SubStep>(id);
        }
    }
}
