using HomeLab.CookBook.Domain.Entities;
using System.Linq.Expressions;

namespace HomeLab.CookBook.Domain.Interfaces.Repositories
{
    public interface IRepository
    {
        Task<T> AddAsync<T>(T entity) where T : Audit;
        Task<T> GetByIdAsync<T>(Guid id, params Expression<Func<T, object>>[] predicates) where T : Audit;
        IQueryable<T> GetAllAsync<T>() where T : Audit;

        Task UpdateAsync<T>(T entity) where T : Audit;
        Task DeleteAsync<T>(Guid id) where T : Audit;

        Task<bool> ExistsAsync<T>(Guid id) where T : Audit;
    }
}
