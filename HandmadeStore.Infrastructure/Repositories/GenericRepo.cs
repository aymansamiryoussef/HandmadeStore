using HandmadeStore.Application.Interfaces;
using HandmadeStore.Application.Interfaces.Repo;
using HandmadeStore.Application.Specifications;
using HandmadeStore.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace HandmadeStore.Infrastructure.Repositories
{
    public class GenericRepo<T> : IGenericRepo<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        
        public GenericRepo(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
            
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public void DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public async Task<IReadOnlyList<T>> GetAllAsync(Expression<Func<T, bool>>? filter)
        {

            IQueryable<T> query = _context.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.ToListAsync();


        }

        public async Task<IReadOnlyList<T>> GetAllWithSepcifications(ISpecification<T> spec)
        {
            IQueryable<T> query = _context.Set<T>();

            foreach (var criteria in spec.Criteria)
                query = query.Where(criteria);

            query = spec.Includes
                .Aggregate(query, (current, include) => current.Include(include));

            if (spec.OrderBy != null)
                query = query.OrderBy(spec.OrderBy);
            else if (spec.OrderByDescending != null)
                query = query.OrderByDescending(spec.OrderByDescending);

            return await query.ToListAsync();


        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T> GetByNameAsync(string name)
        {
            return await _context.Set<T>().FindAsync(name);
        }

        public void UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
        }
    }
}
