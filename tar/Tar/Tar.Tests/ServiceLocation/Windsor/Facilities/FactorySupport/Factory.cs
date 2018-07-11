namespace Tar.Tests.ServiceLocation.Windsor.Facilities.FactorySupport
{
    public class Factory : IFactory
    {
        public IComponent CreateComponent()
        {
            return new Component {Name = "Zahir"};
        }
    }
}