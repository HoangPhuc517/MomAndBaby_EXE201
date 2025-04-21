using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MomAndBaby.Repositories.Helpers;

namespace MomAndBaby.Repositories.Interface
{
    public interface IGenericRepo<T>
    {
        IQueryable<T> GetAll();
        Task<T?> GetByIdAsync(Guid id);
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter, string? includeProperties);
        Task InsertAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        void Update(T entity);
        void Delete(T entity);

        void DeleteRange(List<T> entities);
        Task<T> GetFirstOrDefaultAsync(
            Expression<Func<T, bool>> predicate,
            string? includeProperties = null
        );

        Task<Pagination<T>> GetPaginationAsync(
            Expression<Func<T, bool>>? predicate = null,
            string? includeProperties = null,
            int pageIndex = 0,
            int pageSize = 10,
            Expression<Func<T, object>>? orderBy = null,
            bool isDescending = false
            );

    }
}
