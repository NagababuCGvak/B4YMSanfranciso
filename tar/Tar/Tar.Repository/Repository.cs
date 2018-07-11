using System.Linq;
using Tar.Model;

namespace Tar.Repository
{
    public abstract class Repository<T> : IRepository<T> where T : class, IEntity
    {
        public abstract void Create(T entity);

        public abstract void Update(T entity);

        public abstract void Delete(T entity);

        public abstract void DeleteAll();

        public abstract IQueryable<T> Query();

        public abstract T GetById(object id);
    }
}