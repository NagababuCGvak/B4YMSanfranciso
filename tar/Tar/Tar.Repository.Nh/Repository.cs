using System;
using System.Linq;
using Tar.Model;
using NHibernate;
using NHibernate.Exceptions;
using NHibernate.Linq;

namespace Tar.Repository.Nh
{
    public class Repository<T> : Repository.Repository<T> where T : class, IEntity
    {
        private readonly ISessionHelper _sessionHelper;

        private ISession Session
        {
            get { return _sessionHelper.GetSession(); }
        }

        public Repository(ISessionHelper sessionHelper)
        {
            if (sessionHelper == null) throw new ArgumentNullException("sessionHelper");
            _sessionHelper = sessionHelper;
        }

        public override T GetById(object id)
        {
            return Session.Get<T>(id);
        }

        public override void Create(T entity)
        {
            try
            {
                Session.SaveOrUpdate(entity);
                Session.Flush();
            }
            catch (GenericADOException exception)
            {
                if (exception.Message.ToLower().Contains("conflict") || (exception.InnerException != null
                                                                         &&
                                                                         exception.InnerException.Message.ToLower().
                                                                             Contains("conflict")))
                {
                    throw new ConflictException("ConflictException => EntityName: " + entity.GetType().FullName,
                                                exception.InnerException ?? exception);
                }
                throw;
            }
        }

        public override void Update(T entity)
        {
            Session.SaveOrUpdate(entity);
            Session.Flush();
        }

        public override void Delete(T entity)
        {
            Session.Delete(entity);
            Session.Flush();
        }

        public override void DeleteAll()
        {
            Session.Delete(string.Format("from {0}.{1}", typeof (T).Namespace, typeof (T).Name));
            Session.Flush();
        }

        public override IQueryable<T> Query()
        {
            return Session.Query<T>();
        }
    }
}