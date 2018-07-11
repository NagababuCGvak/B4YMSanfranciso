using System;
using System.Linq;
using Tar.Core;
using Tar.Model;

namespace Tar.Repository.General
{
    public class GeneralRepositoryOld : IGeneralRepository
    {
        private readonly IServiceLocator _serviceLocator;
        private readonly string _innerRepositoryKey;

        public void Create<T>(T entity) where T : class, IEntity
        {
            _serviceLocator.Get<IGeneralRepository<T>>().Create(entity);
        }

        public void Update<T>(T entity) where T : class, IEntity
        {
            _serviceLocator.Get<IGeneralRepository<T>>().Update(entity);
        }

        public void Delete<T>(T entity) where T : class, IEntity
        {
            _serviceLocator.Get<IGeneralRepository<T>>().Delete(entity);
        }

        public void DeleteAll<T>() where T : class, IEntity
        {
            _serviceLocator.Get<IGeneralRepository<T>>().DeleteAll();
        }

        public IQueryable<T> Query<T>() where T : class, IEntity
        {
            return _serviceLocator.Get<IGeneralRepository<T>>().Query();
        }

        public T GetById<T>(object id) where T : class, IEntity
        {
            return _serviceLocator.Get<IGeneralRepository<T>>().GetById(id);
        }

        public GeneralRepositoryOld(IServiceLocator serviceLocator, string innerRepositoryKey)
        {
            if (serviceLocator == null) throw new ArgumentNullException("serviceLocator");
            _serviceLocator = serviceLocator;
            _innerRepositoryKey = innerRepositoryKey;
        }
    }
}
