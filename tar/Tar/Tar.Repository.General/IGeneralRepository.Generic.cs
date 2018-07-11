using Tar.Model;

namespace Tar.Repository.General
{
    public interface IGeneralRepository<T> : IRepository<T> where T : class, IEntity
    {
        IRepository<T> InnerRepository { get; }
    }
}