namespace Tar.Globalization
{
    public class NullResourceManager : IResourceManager
    {
        public string Get(string resourceType, string resourceKey)
        {
            return string.Format("{0}.{1}", resourceType, resourceKey);
        }
    }
}