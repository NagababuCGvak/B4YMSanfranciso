using NHibernate;

namespace Tar.Repository.Nh
{
    public class MultipleSessionHelper : ISessionHelper
    {
        private readonly ISessionHelper _sessionHelper;

        public MultipleSessionHelper(INhRepositoryConfiguration configuration)
        {
            _sessionHelper = new SessionHelper(configuration);
        }

        public void Dispose()
        {
            _sessionHelper.Dispose();
        }

        public ISessionFactory CreateSessionFactory()
        {
            return _sessionHelper.CreateSessionFactory();
        }

        public ISessionFactory GetSessionFactory()
        {
            return _sessionHelper.GetSessionFactory();
        }

        public ISession CreateSession()
        {
            return _sessionHelper.CreateSession();
        }

        public ISession GetSession()
        {
            return _sessionHelper.GetSession();
        }

        public void CloseSession()
        {
            _sessionHelper.CloseSession();
        }
    }
}