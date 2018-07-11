using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Better4You.UI.Mvc.Core
{
    public static class ModelMetadataProviderExtensions
    {
        public static void Add<TModel,TMetadata>(this ModelMetadataProvider provider)
        {
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(
                                                      typeof (TModel),
                                                      typeof (TMetadata)),
                                                  typeof (TModel));
        }
    }
}