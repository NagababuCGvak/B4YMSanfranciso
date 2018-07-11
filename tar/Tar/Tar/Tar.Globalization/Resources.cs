using System;
using System.Threading;

namespace Tar.Globalization
{
    public static class Resources
    {
        public static void SetResourceRepository(IResourceRepository resourceRepository)
        {
            if (resourceRepository == null) throw new ArgumentNullException("resourceRepository");
            _resourceManager = new ResourceManager(resourceRepository);
        }

        private static IResourceManager ResourceManager
        {
            get
            {
                if (_resourceManager == null) throw new SystemException("Not setted resource repository!");
                return _resourceManager;
            }
        }

        private static IResourceManager _resourceManager;

        public static string Field(string resourceKey)
        {
            return Field(Thread.CurrentThread.CurrentUICulture.Name, resourceKey);
        }

        public static string Entity(string resourceKey)
        {
            return Entity(Thread.CurrentThread.CurrentUICulture.Name, resourceKey);
        }

        public static string Error(string resourceKey)
        {
            return Error(Thread.CurrentThread.CurrentUICulture.Name, resourceKey);
        }

        public static string Warning(string resourceKey)
        {
            return Warning(Thread.CurrentThread.CurrentUICulture.Name, resourceKey);
        }

        public static string Info(string resourceKey)
        {
            return Info(Thread.CurrentThread.CurrentUICulture.Name, resourceKey);
        }

        public static string Menu(string resourceKey)
        {
            return Menu(Thread.CurrentThread.CurrentUICulture.Name, resourceKey);
        }

        public static string Field(string language, string resourceKey)
        {
            return ResourceManager.GetResource(language, ResourceManagerParameters.FieldKey, resourceKey);
        }

        public static string Entity(string language, string resourceKey)
        {
            return ResourceManager.GetResource(language, ResourceManagerParameters.EntityKey, resourceKey);
        }

        public static string Error(string language, string resourceKey)
        {
            return ResourceManager.GetResource(language, ResourceManagerParameters.ErrorKey, resourceKey);
        }

        public static string Warning(string language, string resourceKey)
        {
            return ResourceManager.GetResource(language, ResourceManagerParameters.WarningKey, resourceKey);
        }

        public static string Info(string language, string resourceKey)
        {
            return ResourceManager.GetResource(language, ResourceManagerParameters.InfoKey, resourceKey);
        }

        public static string Menu(string language, string resourceKey)
        {
            return ResourceManager.GetResource(language, ResourceManagerParameters.MenuKey, resourceKey);
        }
    }
}