using System;

namespace Mpp_5Lab__DependencyInjectionContainer_
{
    class Dependency
    {
        public Type TDependency { get; private set; }
        public Type TImplementation { get; private set; }
        public Type TInstance { get; private set; }
        public Lifetime lifetime { get; private set; }
        public Dependency(Type dependency, Type implementation, Lifetime lifetime)
        {
            TDependency = dependency;
            TImplementation = implementation;
            this.lifetime = lifetime;
        }        
    }
}