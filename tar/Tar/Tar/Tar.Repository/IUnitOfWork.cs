using System;

namespace Tar.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        bool IsAlive { get; }
        void Commit();
        void Rollback();
    }
}
