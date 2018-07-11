namespace Tar.Tests.ServiceLocation.Windsor.Facilities.FactorySupport
{
    public class Component : IComponent
    {
        public string Name { get; set; }

        public Component()
        {
            Name = "None";
        }
    }
}