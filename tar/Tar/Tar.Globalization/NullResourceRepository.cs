namespace Tar.Globalization
{
    public class NullResourceRepository : IResourceRepository
    {
        public static NullResourceRepository Instance = new NullResourceRepository();

        public string GetResource(string language, string resourceType, string resourceKey)
        {
            return string.Format("{0}.{1}.{2}", language, resourceType, resourceKey);
        }
    }
}