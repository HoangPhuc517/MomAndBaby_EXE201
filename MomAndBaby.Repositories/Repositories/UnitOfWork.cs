using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MomAndBaby.Repositories.ConfigContext;
using MomAndBaby.Repositories.Interface;

namespace MomAndBaby.Repositories.Repositories
{
    public class UnitOfWork(MBContext context) : IUnitOfWork
    {
        private bool disposed = false;
        private readonly MBContext _context = context;

        public IGenericRepo<T> GenericRepository<T>() where T : class
        {
            return new GenericRepo<T>(_context);
        }

        public async Task BeginTransactionAsync()
        {
            await _context.Database.BeginTransactionAsync();
        }

        public async Task<int> SaveChangeAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await _context.Database.CommitTransactionAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            await _context.Database.RollbackTransactionAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            disposed = true;
        }
    }
}
