using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IOC_Container_Tests
{
    public class Class1 : IInterface1
    {
        public Class1(IInterface2 interface2)
        {
            Interface2 = interface2;
        }

        public IInterface2 Interface2 { get; private set; }
    }
}
