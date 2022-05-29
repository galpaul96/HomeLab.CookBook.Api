using HomeLab.CookBook.Domain.Service;

namespace HomeLab.CookBook.Domain.Interfaces.Services
{
    public interface IRecipesService
    {
        Task<RecipeModel> AddAsync(RecipeModel model);
        Task<RecipeModel> GetByIdAsync(Guid id);
        IQueryable<RecipeModel> Get();
        Task<RecipeModel> UpdateAsync(RecipeModel model);
        Task DeleteAsync(Guid id);
    }
}
