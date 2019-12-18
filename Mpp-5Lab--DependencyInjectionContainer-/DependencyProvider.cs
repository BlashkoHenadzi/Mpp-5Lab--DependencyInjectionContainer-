using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mpp_5Lab__DependencyInjectionContainer_
{
    public class DependencyProvider : IDependencyProvider
    {
        private readonly DependenciesConfiguration dependenciesConfiguration;
        private readonly ConcurrentDictionary<int,Stack<Type>> recursionTypes;

        public DependencyProvider(DependenciesConfiguration dependenciesConfiguration)
        {
            this.dependenciesConfiguration = dependenciesConfiguration;
            recursionTypes = new ConcurrentDictionary<int, Stack<Type>>();
        }

        public object Resolve<TDependency>()
         {
            Stack<Type> types;
            if (recursionTypes.TryGetValue(Thread.CurrentThread.ManagedThreadId,out types))
            {
                types.Clear();
            }
            else
            {
                recursionTypes[Thread.CurrentThread.ManagedThreadId] = new Stack<Type>();
            }
            return Resolve(typeof(TDependency));
        }


        private object Resolve(Type type)
        {
            if (type.IsGenericType || type.IsGenericTypeDefinition)
                return ResolveGeneric(type);
            else
                return ResolveSimple(type);
        }

        private object ResolveGeneric(Type type)
        {
            List<object> result = new List<object>();
            IEnumerable<Dependency> dependencies
                = dependenciesConfiguration.GetDependencyImplementations(type)
                .Where(impl => !recursionTypes[Thread.CurrentThread.ManagedThreadId].Contains(impl.TImplementation));

            object instance;
            foreach (Dependency dependencyimpl in dependencies)
            {
                instance = CreateByConstructor(dependencyimpl.TImplementation.GetGenericTypeDefinition()
                    .MakeGenericType(type.GenericTypeArguments));

                if (instance != null)
                {
                    result.Add(instance);
                }
            }

            return result.Count == 1 ? result[0] : result.ToArray(); ;

        }

        private object ResolveSimple(Type type)
        {

            IEnumerable<Dependency> implementationContainers =
                dependenciesConfiguration.GetDependencyImplementations(type)
                .Where(impl => !recursionTypes[Thread.CurrentThread.ManagedThreadId].Contains(impl.TImplementation));

            List<object> result = new List<object>();
            object dependencyInstance;

            foreach (Dependency container in implementationContainers)
            {
                if (container.lifetime == Lifetime.Singleton)
                {
                    if (container.TInstance == null)
                    {
                        lock (container)
                        {
                            if (container.TInstance == null)
                            {
                                container.TInstance = CreateByConstructor(container.TImplementation);
                            }
                        }
                    }
                    dependencyInstance = container.TInstance;
                }
                else
                {
                    dependencyInstance = CreateByConstructor(container.TImplementation);
                }

                if (dependencyInstance != null)
                {
                    result.Add(dependencyInstance);
                }
            }
            return result.Count == 1 ? result[0] : result.ToArray(); ;
        }
            private object CreateByConstructor(Type type)
            {
                ConstructorInfo[] constructors = type.GetConstructors()
                    .OrderBy(constructor => constructor.GetParameters().Length).ToArray();
                object instance = null;
                recursionTypes[Thread.CurrentThread.ManagedThreadId].Push(type);

                for (int constructor = 0; constructor < constructors.Length && instance == null; ++constructor)
                {
                    try
                    {
                        List<object> parameters = new List<object>();
                        foreach (ParameterInfo constructorParameter in constructors[constructor].GetParameters())
                        {
                            parameters.Add(Resolve(constructorParameter.ParameterType));
                        }
                    object[] para = parameters.ToArray();
                        instance = constructors[constructor].Invoke(para);
                    }
                    catch(Exception e)
                    {
                    
                    }
                }

                recursionTypes[Thread.CurrentThread.ManagedThreadId].Pop();
                return instance;
            }
    }
}
