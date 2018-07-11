namespace Tar.Globalization
{
    public static class ResourceManagerExtensions
    {
        public static string Field(this IResourceManager resourceManager, string resourceKey)
        {
            return resourceManager.Get(ResourceManagerParameters.FieldKey, resourceKey);
        }
        
        public static string Entity(this IResourceManager resourceManager, string resourceKey)
        {
            return resourceManager.Get(ResourceManagerParameters.EntityKey, resourceKey);
        }

        public static string Alert(this IResourceManager resourceManager, string resourceKey)
        {
            return resourceManager.Get(ResourceManagerParameters.AlertKey, resourceKey);
        }
        
        public static string Warning(this IResourceManager resourceManager, string resourceKey)
        {
            return resourceManager.Get(ResourceManagerParameters.WarningKey, resourceKey);
        }

        public static string Info(this IResourceManager resourceManager, string resourceKey)
        {
            return resourceManager.Get(ResourceManagerParameters.InfoKey, resourceKey);
        }

        public static string Menu(this IResourceManager resourceManager, string resourceKey)
        {
            return resourceManager.Get(ResourceManagerParameters.MenuKey, resourceKey);
        }
    }
}