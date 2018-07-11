namespace Tar.Repository
{
    public interface IRepositoryConfiguration
    {
        bool Configured { get; }
        void Configure();
    }
}
