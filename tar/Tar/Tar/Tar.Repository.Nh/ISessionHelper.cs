using System;
using NHibernate;

namespace Tar.Repository.Nh
{
    public interface ISessionHelper : IDisposable
    {
        ISessionFactory CreateSessionFactory();

        ISessionFactory GetSessionFactory();

        ISession CreateSession();

        ISession GetSession();

        void CloseSession();
    }
}