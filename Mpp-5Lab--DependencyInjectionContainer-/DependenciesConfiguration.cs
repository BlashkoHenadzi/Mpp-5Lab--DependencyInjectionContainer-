using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Mpp_5Lab__DependencyInjectionContainer_
{
    public class DependenciesConfiguration : IDependencyConfiguration
    {
        ConcurrentDictionary<Type,List<Dependency>> RegisteredDependecies;

        public DependenciesConfiguration()
        {
            RegisteredDependecies = new ConcurrentDictionary<Type, List<Dependency>>();
        }

        public void Register<TDependency, TImplementation>(Lifetime lifetime)
            where TDependency : class
            where TImplementation : TDependency
        {
            Register(typeof(TDependency), typeof(TImplementation), lifetime);
        }

        public void Register(Type dependency, Type dependencyimpl, Lifetime lifetime)
        {
            if (dependency.IsGenericType)
            {
                dependency = dependency.GetGenericTypeDefinition();
            }
            List<Dependency> _dependencies;
            RegisteredDependecies.TryGetValue(dependency, out _dependencies);
            if (_dependencies == null)            {
               _dependencies = new List<Dependency>();               
               RegisteredDependecies[dependency] = _dependencies;
            }
            lock (_dependencies)
            {
                Dependency _dependency = new Dependency(dependency, dependencyimpl, lifetime);
                _dependencies.Add(_dependency);
            }
        }
        public IEnumerable<Dependency> GetDependencyImplementations(Type type)
        {
            Type collectionType;

            if (type.IsGenericType)
            {
                collectionType = type.GetGenericTypeDefinition();
            }
            else
            {
                collectionType = type;
            }

            if (RegisteredDependecies.TryGetValue(collectionType,
                out List<Dependency> dependencyImplementations))
            {
                IEnumerable<Dependency> result =
                    new List<Dependency>(dependencyImplementations);
                if (type.IsGenericType)
                {
                    result = result.Where(impl => impl.TImplementation.IsGenericTypeDefinition
                                                    || type.IsAssignableFrom(impl.TImplementation));
                }

                return result;
            }
            return new List<Dependency>();
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

