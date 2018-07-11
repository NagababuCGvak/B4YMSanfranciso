using Castle.MicroKernel;
using Castle.MicroKernel.ComponentActivator;

namespace Tar.Core.Windsor.ComponentActivator
{
    public class BuildUpComponentActivator : DefaultComponentActivator
    {
        public const string Key = "BuildUpComponentActivator.Key";

        public BuildUpComponentActivator(Castle.Core.ComponentModel model, IKernel kernel, ComponentInstanceDelegate onCreation, ComponentInstanceDelegate onDestruction)
            : base(model, kernel, onCreation, onDestruction)
        {
        }

        protected override object Instantiate(Castle.MicroKernel.Context.CreationContext context)
        {
            return context.AdditionalArguments.Contains(Key)
                       ? context.AdditionalArguments[Key]
                       : base.Instantiate(context);
        }
    }
}
