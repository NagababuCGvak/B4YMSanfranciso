using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Tar.Core
{
    public class WcfObjectContainerExtension : Dictionary<string, object>, IExtension<IContextChannel>, IExtension<InstanceContext>, IExtension<OperationContext>
    {
        public void Attach(IContextChannel owner)
        {
        }

        public void Detach(IContextChannel owner)
        {
        }

        public void Attach(InstanceContext owner)
        {
            //throw new System.NotImplementedException();
        }

        public void Detach(InstanceContext owner)
        {
            //throw new System.NotImplementedException();
        }

        public void Attach(OperationContext owner)
        {
            //throw new System.NotImplementedException();
        }

        public void Detach(OperationContext owner)
        {
            //throw new System.NotImplementedException();
        }
    }
}