using System.Linq;
using Tar.Model;

namespace Tar.Repository.General.Multiple
{
    public interface IMultipleRepository<in TEntity>
        where TEntity : IMultipleRepositoryEntity
    {
        void Create<T>(T entity) where T : class, TEntity;
        void Update<T>(T entity) where T : class, TEntity;
        void Delete<T>(T entity) where T : class, TEntity;
        void DeleteAll<T>() where T : class, TEntity;
        IQueryable<T> Query<T>() where T : class, TEntity;
        T GetById<T>(object id) where T : class, TEntity;
    }
}