namespace Tar.Repository
{
    public class UnitOfWorkCounter : IUnitOfWorkCounter
    {
        public void Increase()
        {
            Count++;
        }

        public void Decrease()
        {
            Count--;
        }

        public int Count { get; protected set; }
    }
}