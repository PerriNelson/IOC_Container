using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IOC_Container
{
    public class TypeResolverContext
    {
        public IList<ITypeRegistration> RegisteredTypes { get; set; }
    }
}
