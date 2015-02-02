using System;

namespace IOC_Container
{
    public class TypeRegistration : ITypeRegistration
    {
        public Type InterfaceType {get; private set;}
        public Type ConcreteType {get; private set;}
        public LifeStyleType LifeStyle {get; private set;}

        public TypeRegistration(Type interfaceType, Type concreteType, LifeStyleType lifeStyle)
        {
            if (null == interfaceType)
                throw new ArgumentNullException("interfaceType");
            if (null == concreteType)
                throw new ArgumentNullException("concreteType");
            InterfaceType = interfaceType;
            ConcreteType = concreteType;
            LifeStyle = lifeStyle;
        }
    }
}
