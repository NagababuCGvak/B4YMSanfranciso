using System;
using System.Collections.Generic;

namespace Tar.Globalization
{
    public class ResourceManager : IResourceManager
    {
        private readonly IDictionary<string, string> _resources = new Dictionary<string, string>();

        private readonly IResourceRepository _resourceRepository;

        public ResourceManager(IResourceRepository resourceRepository)
        {
            if (resourceRepository == null) throw new ArgumentNullException("resourceRepository");
            _resourceRepository = resourceRepository;
        }

        public string GetResource(string resourceLanguage, string resourceType, string resourceKey)
        {
            var key = string.Format("{0}.{1}.{2}", resourceLanguage, resourceType, resourceKey);

            lock (this)
            {
                if (!_resources.ContainsKey(key))
                {
                    var result = _resourceRepository.GetResource(resourceLanguage, resourceType, resourceKey);
                    _resources.Add(key, result);
                }
            }
            return _resources[key];;
        }
    }
}