using System;
using System.Data;
using System.Linq;
using Tar.Model;

namespace Tar.Repository.Ef
{
    public class Repository<T> : Tar.Repository.Repository<T> where T : class, IEntity
    {
        private readonly EfDbContext _context;

        public Repository(EfDbContext context)
        {   
            if (context == null) throw new ArgumentNullException("context");
            _context = context;
        }

        public override T GetById(object id)
        {
            return _context.Set<T>().Find(id);
        }

        public override void Create(T entity)
        {
            try
            {
                _context.Set<T>().Add(entity);
                _context.SaveChanges();
            }
            catch (Exception exception)
            {
                if (exception.InnerException != null)
                    if (exception.InnerException.InnerException != null)
                        if (exception.InnerException.InnerException.Message.Contains("conflicted"))
                            throw new ConflictException(
                                "ConflictException => EntityName: " + entity.GetType().FullName,
                                exception.InnerException);
                throw;
            }
        }

        public override void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public override void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
        }

        public override void DeleteAll()
        {
            _context.Set<T>().ToList().ForEach(Delete);
        }

        public override IQueryable<T> Query()
        {
            return _context.Set<T>();
        }
    }
}