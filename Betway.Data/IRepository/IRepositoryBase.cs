using Betway.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Betway.Data.IRepository
{
    public interface IRepositoryBase<T> : IDisposable where T : ModelBase
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T> Get(int id);

        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>>? where = null,
            Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null);

        Task<IEnumerable<T>> GetAsync(int pageNumber, int pageSize);

        Task<T> InsertAsync(T entity);

        Task<List<T>> InsertRangeAsync(List<T> entities);

        Task<T> UpdateAsync(T entity);

        T Delete(T entity);
    }
}
