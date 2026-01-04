using HandmadeStore.Application.Specifications;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace HandmadeStore.Application.Interfaces.Repo
{
    public interface IGenericRepo<T> where T : class
    {
        public Task<T> GetByIdAsync(int id);
        public Task<T> GetByNameAsync(string name);
        public Task<IReadOnlyList<T>> GetAllAsync(Expression<Func<T,bool>>? filter=null);
        public Task<IReadOnlyList<T>> GetAllWithSepcifications(ISpecification<T> spec);
        public Task AddAsync(T entity);

        public void UpdateAsync(T entity);

        public void DeleteAsync(T entity);

    }
}
