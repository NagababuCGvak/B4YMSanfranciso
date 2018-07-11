using System.Linq;
using Tar.Core;
using Tar.Model;

namespace Tar.Repository.General.Multiple
{
    public class MultipleRepository<TEntity> :IMultipleRepository<TEntity>
        where TEntity : IMultipleRepositoryEntity
    {
        private readonly IGeneralRepository _general;
        public MultipleRepository(IServiceLocator serviceLocator, string innerRepositoryKey)
        {
            _general = new GeneralRepository(serviceLocator, innerRepositoryKey);
        }

        public void Create<T>(T entity) where T : class, TEntity
        {
            _general.Create(entity);
        }

        public void Update<T>(T entity) where T : class, TEntity
        {
            _general.Update(entity);
        }

        public void Delete<T>(T entity) where T : class, TEntity
        {
            _general.Delete(entity);
        }

        public void DeleteAll<T>() where T : class, TEntity
        {
            _general.DeleteAll<T>();
        }

        public IQueryable<T> Query<T>() where T : class, TEntity
        {
            return _general.Query<T>();
        }

        public T GetById<T>(object id) where T : class, TEntity
        {
            return _general.GetById<T>(id);
        }
    }
}