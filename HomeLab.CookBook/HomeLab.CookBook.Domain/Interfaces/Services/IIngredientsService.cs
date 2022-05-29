using HomeLab.CookBook.Domain.Service;

namespace HomeLab.CookBook.Domain.Interfaces.Services
{
    public interface IIngredientsService
    {
        Task<IngredientModel> AddAsync(IngredientModel model);
        Task<IngredientModel> GetByIdAsync(Guid id);
        IQueryable<IngredientModel> Get();
        Task<IngredientModel> UpdateAsync(IngredientModel model);
        Task DeleteAsync(Guid id);
    }
}
