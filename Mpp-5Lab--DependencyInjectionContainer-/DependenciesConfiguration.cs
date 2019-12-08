using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpp_5Lab__DependencyInjectionContainer_
{
    class DependenciesConfiguration : IDependencyConfiguration
    {
        List<Dependency> RegisteredDependecies;

        public DependenciesConfiguration()
        {
            RegisteredDependecies = new List<Dependency>();
        }

        public void Register<TDependency, TImplementation>(Lifetime lifetime)
            where TDependency : class
            where TImplementation : TDependency
        {
            throw new NotImplementedException();
        }

        public void Register(Type service, Type serviceimpl, Lifetime lifetime)
        {
            throw new NotImplementedException();
        }
    }
}
