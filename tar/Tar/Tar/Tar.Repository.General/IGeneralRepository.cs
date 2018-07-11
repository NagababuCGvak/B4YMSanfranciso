using System.Linq;
using Tar.Model;

namespace Tar.Repository.General
{
    public interface IGeneralRepository
    {
        void Create<T>(T entity) where T : class, IEntity;
        void Update<T>(T entity) where T : class, IEntity;
        void Delete<T>(T entity) where T : class, IEntity;
        void DeleteAll<T>() where T : class, IEntity;
        IQueryable<T> Query<T>() where T : class, IEntity;
        T GetById<T>(object id) where T : class, IEntity;
    }
}
