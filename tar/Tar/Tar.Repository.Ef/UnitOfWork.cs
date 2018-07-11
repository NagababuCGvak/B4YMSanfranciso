using System.Data.Common;

namespace Tar.Repository.Ef
{
    public class UnitOfWork : Repository.UnitOfWork
    {
        private readonly DbTransaction _transaction;
        private DbConnection _dbConnection;

        public UnitOfWork(IUnitOfWorkCounter counter, EfDbContext dbContext)
            : base(counter)
        {
            if (counter.Count > 1) return;
            _dbConnection = dbContext.ObjectContext.Connection;

            _dbConnection.Open();
            _transaction = _dbConnection.BeginTransaction();
        }

        protected override void DoCommit()
        {
            _transaction.Commit();
            _transaction.Dispose();
            _dbConnection.Close();
        }

        protected override void DoRollback()
        {
            _transaction.Rollback();
            _transaction.Dispose();
            _dbConnection.Close();
        }
    }
}