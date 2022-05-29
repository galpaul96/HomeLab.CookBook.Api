using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HomeLab.CookBook.Domain.Entities;
using HomeLab.CookBook.Domain.Interfaces.Repositories;
using HomeLab.CookBook.EF.Infra;
using Microsoft.EntityFrameworkCore;

namespace HomeLab.CookBook.EF.Repositories
{
    internal class Repository : IRepository
    {
        private readonly CookBookContext _context;

        public Repository(CookBookContext context)
        {
            _context = context;
        }

        public async Task<T> AddAsync<T>(T entity) where T : Audit
        {
            entity.CreatedDate = DateTime.UtcNow;
            var result = await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<T> GetByIdAsync<T>(Guid id, params Expression<Func<T, object>>[] predicates) where T : Audit
        {
            var result = _context.Set<T>().AsQueryable();

            foreach (var predicate in predicates)
            {
                result = result.Include(predicate);
            }

            return await result.FirstOrDefaultAsync(x => x.Id == id);
        }

        public IQueryable<T> GetAllAsync<T>() where T : Audit
        {
            return _context.Set<T>().AsQueryable();
            
        }

        public async Task UpdateAsync<T>(T entity) where T : Audit
        {
            entity.UpdatedDate =DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync<T>(Guid id) where T : Audit
        {
            var entityToDelete = await _context.Set<T>().FirstAsync(x => x.Id == id);
            _context.Set<T>().Remove(entityToDelete);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync<T>(Guid id) where T : Audit
        {
            return await _context.Set<T>().AnyAsync(x => x.Id == id);
        }
        
    }
}
