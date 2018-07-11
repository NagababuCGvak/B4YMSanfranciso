namespace Tar.Core.ObjectContainers
{
    public class StaticObjectContainer : ObjectContainerBase
    {
        private static readonly ObjectContainer Container = new ObjectContainer();

        public override void DoSet(string key, object value)
        {
            Container.Set(key, value);
        }

        public override object DoGet(string key)
        {
            return Container.Get(key);
        }
    }
}