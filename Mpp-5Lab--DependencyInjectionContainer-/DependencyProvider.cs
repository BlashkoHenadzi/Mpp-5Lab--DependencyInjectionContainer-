using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpp_5Lab__DependencyInjectionContainer_
{
    class DependencyProvider : IDependencyProvider
    {
        TDependency IDependencyProvider.Resolve<TDependency>()
        {
            throw new NotImplementedException();
        }
    }
}
