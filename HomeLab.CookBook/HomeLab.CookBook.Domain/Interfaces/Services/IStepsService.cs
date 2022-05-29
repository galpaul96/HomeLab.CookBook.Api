using HomeLab.CookBook.Domain.Service;

namespace HomeLab.CookBook.Domain.Interfaces.Services
{
    public interface IStepsService
    {
        Task<StepModel> AddAsync(StepModel recipe);
        Task<StepModel> GetByIdAsync(Guid id);
        IQueryable<StepModel> Get();
        Task<StepModel> UpdateAsync(StepModel step);
        Task DeleteAsync(Guid id);
    }
}
