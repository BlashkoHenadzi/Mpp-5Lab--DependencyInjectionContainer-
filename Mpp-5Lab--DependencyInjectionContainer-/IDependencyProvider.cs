using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpp_5Lab__DependencyInjectionContainer_
{
    interface IDependencyProvider
    {
        TDependency Resolve<TDependency>() where TDependency : class; 
    }
}
