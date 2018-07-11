using Tar.Core;

namespace Tar.Repository.Nh
{
    public class BootStrapper : IBootStrapper
    {
        private readonly IServiceLocator _serviceLocator;

        public BootStrapper(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
        }

        public void BootStrap()
        {
        }
    }
}
