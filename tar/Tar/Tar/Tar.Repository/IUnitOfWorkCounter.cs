namespace Tar.Repository
{
    public interface IUnitOfWorkCounter
    {
        void Increase();
        void Decrease();
        int Count { get; }
    }
}