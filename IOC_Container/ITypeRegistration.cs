using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IOC_Container
{
    public interface ITypeRegistration
    {
        Type InterfaceType { get; }
        Type ConcreteType { get; }
        LifeStyleType LifeStyle { get; }
    }
}
