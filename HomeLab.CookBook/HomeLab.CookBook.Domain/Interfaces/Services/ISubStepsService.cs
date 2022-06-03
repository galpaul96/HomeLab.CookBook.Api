using HomeLab.CookBook.Domain.Service;

namespace HomeLab.CookBook.Domain.Interfaces.Services
{
    public interface ISubStepsService
    {
        Task<SubStepModel> AddAsync(SubStepModel recipe);
        Task<SubStepModel> GetByIdAsync(Guid id);
        IQueryable<SubStepModel> Get();
        Task<SubStepModel> UpdateAsync(SubStepModel step);
        Task DeleteAsync(Guid id);
    }
}
