using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomAndBaby.Repositories.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepo<T> GenericRepository<T>() where T : class;

        Task<int> SaveChangeAsync();

        Task BeginTransactionAsync();

        Task CommitTransactionAsync();

        Task RollbackTransactionAsync();
    }
}
