using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace School.Data.DataAccess.Repositories.Generic
{
    public interface IRepository<T>
    {
        Task<T> AddAsync(T entity);
        T Update(T entity);
        Task<T> FindByIdAsync(string Id);
        Task<IEnumerable<T>> AllAsync();
        Task<T> FindAsync(Expression<Func<T, bool>> predicate);
        void Delete(T entity);
        Task SaveChangesAsync();
    }
}