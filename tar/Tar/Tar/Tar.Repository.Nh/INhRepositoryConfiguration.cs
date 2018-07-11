using NHibernate;
using NHibernate.Cfg;

namespace Tar.Repository.Nh
{
    public interface INhRepositoryConfiguration : IRepositoryConfiguration
    {
        Configuration Configuration { get; }
        void CreateSchema();
        ISessionFactory CreateSessionFactory();
        ISessionFactory GetSessionFactory();
    }
}