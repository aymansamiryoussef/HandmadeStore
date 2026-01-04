using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace HandmadeStore.Application.Specifications
{
    public abstract class BaseSpecification<T> : ISpecification<T>
    {
        protected BaseSpecification()
        {
            Criteria = new List<Expression<Func<T, bool>>>();
        }

        public List<Expression<Func<T, bool>>> Criteria { get; }
        public List<Expression<Func<T, object>>> Includes { get; } = new();

        public Expression<Func<T, object>>? OrderBy { get; private set; }
        public Expression<Func<T, object>>? OrderByDescending { get; private set; }

        protected void AddCriteria(Expression<Func<T, bool>> criteria)
            => Criteria.Add(criteria);

        protected void AddInclude(Expression<Func<T, object>> include)
        => Includes.Add(include);

        protected void AddOrderBy(Expression<Func<T, object>> orderBy)
            => OrderBy = orderBy;

        protected void AddOrderByDescending(Expression<Func<T, object>> orderByDesc)
            => OrderByDescending = orderByDesc;

    }
}
