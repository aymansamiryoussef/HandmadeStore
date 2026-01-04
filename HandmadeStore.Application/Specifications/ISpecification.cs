using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace HandmadeStore.Application.Specifications
{
    public interface ISpecification<T> 
    {
        List<Expression<Func<T, bool>>> Criteria { get; }


        List<Expression<Func<T, object>>> Includes { get; }

        Expression<Func<T, object>>? OrderBy { get; }
        Expression<Func<T, object>>? OrderByDescending { get; }

    }
}
