using System;
using NHibernate;

namespace Tar.Repository.Nh
{
    public class SessionHelper : ISessionHelper
    {
        private readonly INhRepositoryConfiguration _configuration;

        public SessionHelper(INhRepositoryConfiguration configuration)
        {
            _configuration = configuration;
            if (!configuration.Configured)
                configuration.Configure();
        }

        private ISession _session;

        public ISessionFactory CreateSessionFactory()
        {
            return _configuration.CreateSessionFactory();
        }

        public ISessionFactory GetSessionFactory()
        {
            return _configuration.GetSessionFactory();
        }

        public ISession CreateSession()
        {
            return GetSessionFactory().OpenSession();
        }

        public ISession GetSession()
        {
            if (_session == null)
                _session = CreateSession();

            if (!_session.IsConnected)
                _session.Reconnect();

            return _session;
        }

        public void CloseSession()
        {
            _session.Dispose();
            _session = null;
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            if (_session != null)
                CloseSession();
            GC.SuppressFinalize(this);
        }

        #endregion Implementation of IDisposable
    }
}