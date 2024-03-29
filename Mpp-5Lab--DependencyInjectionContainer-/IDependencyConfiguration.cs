﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpp_5Lab__DependencyInjectionContainer_
{
    public enum Lifetime { InstPerDep, Singleton}
    
    interface IDependencyConfiguration
    {
        void Register<TDependency, TImplementation>(Lifetime lifetime) where TDependency : class
                                                      where TImplementation : TDependency;
        void Register(Type dependency, Type dependencyimpl, Lifetime lifetime);
    }
}
