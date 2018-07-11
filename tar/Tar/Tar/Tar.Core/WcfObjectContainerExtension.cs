using System.Collections.Generic;
using System.ServiceModel;

namespace Tar.Core
{
    public class WcfObjectContainerExtension:Dictionary<string,object>, IExtension<IContextChannel>
    {
        public void Attach(IContextChannel owner)
        {
        }

        public void Detach(IContextChannel owner)
        {
        }
    }
}