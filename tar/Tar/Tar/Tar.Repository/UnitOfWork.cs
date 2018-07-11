using System;

namespace Tar.Repository
{
    public abstract class UnitOfWork : IUnitOfWork
    {
        private readonly IUnitOfWorkCounter _counter;
        public bool IsAlive { get; protected set; }
        
        public void Commit()
        {
            if (_counter.Count != 1) return;

            IsAlive = false;
            DoCommit();
        }

        protected abstract void DoCommit();
        protected abstract void DoRollback();

        public void Rollback()
        {
            if (_counter.Count != 1) return;
            IsAlive = false;
            DoRollback();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected UnitOfWork(IUnitOfWorkCounter counter)
        {
            _counter = counter;
            _counter.Increase();
            IsAlive = true;
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (IsAlive)
                    Rollback();
                _counter.Decrease();
            }
        }
    }
}