using System.Linq;
using Tar.Model;

namespace Tar.Repository
{
    public interface IRepository<T> where T : class, IEntity
    {
        void Create(T entity);

        void Update(T entity);

        void Delete(T entity);

        void DeleteAll();

        IQueryable<T> Query();

        T GetById(object id);
    }
}