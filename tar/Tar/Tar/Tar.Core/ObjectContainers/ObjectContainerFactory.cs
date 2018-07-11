namespace Tar.Core.ObjectContainers
{
    public static class ObjectContainerFactory
    {
        public static IObjectContainer Create(ObjectContainerType objectContainerType)
        {
            switch (objectContainerType)
            {
                case ObjectContainerType.Environment:
                    return new EnvironmentObjectContainer();
                case ObjectContainerType.Static:
                    return new StaticObjectContainer();
                case ObjectContainerType.WebRequest:
                    return new WebRequestObjectContainer();
                case ObjectContainerType.Thread:
                    return new ThreadObjectContainer();
                case ObjectContainerType.WcfRequest:
                    return new WcfRequestObjectContainer();
                default:
                    return new ObjectContainer();
            }
        }
    }
}
