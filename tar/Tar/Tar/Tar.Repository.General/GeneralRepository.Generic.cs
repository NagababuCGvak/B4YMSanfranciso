using System.Linq;
using Tar.Core;
using Tar.Model;

namespace Tar.Repository.General
{
    public class GeneralRepository<T> : Repository<T>, IGeneralRepository<T> where T : class, IEntity
    {
        public override void Create(T entity)
        {
            InnerRepository.Create(entity);
        }

        public override void Update(T entity)
        {
            InnerRepository.Update(entity);
        }

        public override void Delete(T entity)
        {
            InnerRepository.Delete(entity);
        }

        public override void DeleteAll()
        {
            InnerRepository.DeleteAll();
        }

        public override IQueryable<T> Query()
        {
            return InnerRepository.Query();
        }

        public override T GetById(object id)
        {
            return InnerRepository.GetById(id);
        }

        /// <summary>
        /// Use nh for NHibernate or ef for Entity Framework.
        /// </summary>
        /// <param name="innerRepositoryKey">Use nh for NHibernate or ef for Entity Framework.</param>
        public GeneralRepository(string innerRepositoryKey)
        {
            InnerRepository = ServiceLocator.Current.Get<IRepository<T>>(innerRepositoryKey);
        }

        public IRepository<T> InnerRepository { get; private set; }
    }
}