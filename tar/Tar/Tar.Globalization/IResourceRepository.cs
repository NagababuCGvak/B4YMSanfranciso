namespace Tar.Globalization
{
    public interface IResourceRepository
    {
        string GetResource(string resourceLanguage, string resourceType, string resourceKey);
    }
}
