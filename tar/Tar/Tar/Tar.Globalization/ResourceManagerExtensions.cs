using System.Threading;

namespace Tar.Globalization
{
    public static class ResourceManagerExtensions
    {
        public static string Field(this IResourceManager resourceManager, string resourceKey)
        {
            return resourceManager.GetResource(Thread.CurrentThread.CurrentUICulture.Name, ResourceManagerParameters.FieldKey, resourceKey);
        }
        
        public static string Entity(this IResourceManager resourceManager, string resourceKey)
        {
            return resourceManager.GetResource(Thread.CurrentThread.CurrentUICulture.Name, ResourceManagerParameters.EntityKey, resourceKey);
        }

        public static string Error(this IResourceManager resourceManager, string resourceKey)
        {
            return resourceManager.GetResource(Thread.CurrentThread.CurrentUICulture.Name, ResourceManagerParameters.ErrorKey, resourceKey);
        }
        
        public static string Warning(this IResourceManager resourceManager, string resourceKey)
        {
            return resourceManager.GetResource(Thread.CurrentThread.CurrentUICulture.Name, ResourceManagerParameters.WarningKey, resourceKey);
        }

        public static string Info(this IResourceManager resourceManager, string resourceKey)
        {
            return resourceManager.GetResource(Thread.CurrentThread.CurrentUICulture.Name, ResourceManagerParameters.InfoKey, resourceKey);
        }

        public static string Menu(this IResourceManager resourceManager, string resourceKey)
        {
            return resourceManager.GetResource(
                Thread.CurrentThread.CurrentUICulture.Name,
                ResourceManagerParameters.MenuKey,
                resourceKey);
        }

        public static string Field(this IResourceManager resourceManager, string language, string resourceKey)
        {
            return resourceManager.GetResource(language, ResourceManagerParameters.FieldKey, resourceKey);
        }

        public static string Entity(this IResourceManager resourceManager, string language, string resourceKey)
        {
            return resourceManager.GetResource(language, ResourceManagerParameters.EntityKey, resourceKey);
        }

        public static string Error(this IResourceManager resourceManager, string language, string resourceKey)
        {
            return resourceManager.GetResource(language, ResourceManagerParameters.ErrorKey, resourceKey);
        }

        public static string Warning(this IResourceManager resourceManager, string language, string resourceKey)
        {
            return resourceManager.GetResource(language, ResourceManagerParameters.WarningKey, resourceKey);
        }

        public static string Info(this IResourceManager resourceManager, string language, string resourceKey)
        {
            return resourceManager.GetResource(language, ResourceManagerParameters.InfoKey, resourceKey);
        }

        public static string Menu(this IResourceManager resourceManager, string language, string resourceKey)
        {
            return resourceManager.GetResource(language,
                ResourceManagerParameters.MenuKey,
                resourceKey);
        }
    }
}