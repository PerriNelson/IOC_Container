using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IOC_Container
{
    public interface ILifeStyle
    {
        LifeStyleType Type { get; }

        TInterface Resolve<TInterface>(TypeResolverContext context);
    }
}
