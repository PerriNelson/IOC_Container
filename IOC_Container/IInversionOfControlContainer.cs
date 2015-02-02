using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IOC_Container
{
    public interface IInversionOfControlContainer
    {
        void Register<TInterface, TClass>(LifeStyleType lifeStyle = LifeStyleType.Transient)
            where TInterface : class
            where TClass : TInterface;
        TInterface Resolve<TInterface>()
            where TInterface : class;
    }
}
