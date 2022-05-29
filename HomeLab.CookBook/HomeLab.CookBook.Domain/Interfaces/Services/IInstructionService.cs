using HomeLab.CookBook.Domain.Service;

namespace HomeLab.CookBook.Domain.Interfaces.Services
{
    public interface IInstructionService
    {
        Task<InstructionModel> AddAsync(InstructionModel recipe);
        Task<InstructionModel> GetByIdAsync(Guid id);
        IQueryable<InstructionModel> Get();
        Task<InstructionModel> UpdateAsync(InstructionModel step);
        Task DeleteAsync(Guid id);
    }
}
