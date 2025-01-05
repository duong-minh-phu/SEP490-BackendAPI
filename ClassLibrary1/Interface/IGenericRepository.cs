using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.Interface
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] includeProperties);
        Task<T> GetByIdAsync(object id, params Expression<Func<T, object>>[] includeProperties);

        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
