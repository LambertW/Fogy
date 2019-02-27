using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fogy.Core.Domain.Uow
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        public Guid Id { get; }

        private IDbConnection _connection;
        public IDbConnection Connection
        {
            get
            {
                return _connection;
            }
        }

        private IDbTransaction _transaction;
        public IDbTransaction Transaction
        {
            get
            {
                return _transaction;
            }
        }

        public UnitOfWork(IDbConnectionProvider provider)
        {
            Id = Guid.NewGuid();
            _connection = provider.GetDbConnection();
        }

        public void Begin()
        {
            _transaction = Connection.BeginTransaction();
        }

        public void Rollback()
        {
            _transaction.Rollback();
            Dispose();
        }

        private bool _disposed;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if(disposing)
                {
                    _transaction?.Dispose();
                }

                _disposed = true;
            }
        }

        public void SaveChanges()
        {
            try
            {
                _transaction.Commit();
            }
            catch
            {
                _transaction.Rollback();
                throw;
            }

            Dispose();
        }
    }
}
