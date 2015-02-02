using System;
using System.Collections.Generic;
using IOC_Container;

namespace IOC_Container_Tests
{
    public class TestingInversionOfControlContainer : InversionOfControlContainer
    {
        public TestingInversionOfControlContainer(IDictionary<Type, ITypeRegistration> typeRegistry)
        {
            InitializeTypeRegistry(typeRegistry);
        }
    }
}
