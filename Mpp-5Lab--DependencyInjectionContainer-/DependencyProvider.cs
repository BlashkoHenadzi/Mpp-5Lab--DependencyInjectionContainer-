using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mpp_5Lab__DependencyInjectionContainer_
{
    class DependencyProvider : IDependencyProvider
    {
        private readonly DependenciesConfiguration dependenciesConfiguration;
        private readonly ConcurrentDictionary<int,Stack<Type>> recursionTypes;

        public DependencyProvider(DependenciesConfiguration dependenciesConfiguration)
        {
            this.dependenciesConfiguration = dependenciesConfiguration;
            recursionTypes = new ConcurrentDictionary<int, Stack<Type>>();
        }

        TDependency IDependencyProvider.Resolve<TDependency>()
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
            return (TDependency)Resolve(typeof(TDependency));
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
            throw new NotImplementedException();
        }

        private object ResolveSimple(Type type)
        {
            throw new NotImplementedException();
        }
    }
}
