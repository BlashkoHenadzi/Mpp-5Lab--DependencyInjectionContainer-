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
            Register(typeof(TDependency), typeof(TImplementation), lifetime);
        }

        public void Register(Type dependency, Type dependencyimpl, Lifetime lifetime)
        {
            if (Validate(dependency, dependencyimpl) && RegisteredDependecies.Find(x => (x.TDependency == dependency && x.TImplementation == dependencyimpl)) == null)
            {
                Dependency _dependency = new Dependency(dependency, dependencyimpl, lifetime);
                RegisteredDependecies.Add(_dependency);
            }
            else
                throw new Exception("Dependency already registered");

        }
        public bool Validate(Type dependency, Type dependencyimpl)
        {
            if (dependencyimpl.IsAbstract || dependencyimpl.IsAbstract || !dependency.IsAssignableFrom(dependencyimpl))
            {
                throw new Exception("Incorrect dependency");
            }
            return true;
        }
    }
}

