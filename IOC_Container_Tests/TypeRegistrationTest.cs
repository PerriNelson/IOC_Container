using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using IOC_Container;

namespace IOC_Container_Tests
{
    [TestFixture]
    public class TypeRegistrationTest
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException), ExpectedMessage=@"Value cannot be null.
Parameter name: interfaceType")]
        public void Test_WhenNullPassedForInterfaceType_ThrowsArgumentNullException()
        {
            TypeRegistration result = new TypeRegistration(null, null, LifeStyleType.Transient);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException), ExpectedMessage = @"Value cannot be null.
Parameter name: concreteType")]
        public void Test_WhenNullPassedForConcreteType_ThrowsArgumentNullException()
        {
            TypeRegistration result = new TypeRegistration(typeof(ITypeRegistration), null, LifeStyleType.Transient);
        }

        [Test]
        public void Test_RetrievingInterfaceType_ReturnsValuePassedToConstructor()
        {
            TypeRegistration systemUnderTest = new TypeRegistration(typeof(ITypeRegistration), typeof(TypeRegistration), LifeStyleType.Transient);
            Assert.That(systemUnderTest.InterfaceType, Is.EqualTo(typeof(ITypeRegistration)));
        }

        [Test]
        public void Test_RetrievingConcreteType_ReturnsValuePassedToConstructor()
        {
            TypeRegistration systemUnderTest = new TypeRegistration(typeof(ITypeRegistration), typeof(TypeRegistration), LifeStyleType.Transient);
            Assert.That(systemUnderTest.ConcreteType, Is.EqualTo(typeof(TypeRegistration)));
        }

        [Test]
        public void Test_RetrievingLifeStyle_ReturnsValuePassedToConstructor()
        {
            TypeRegistration systemUnderTest = new TypeRegistration(typeof(ITypeRegistration), typeof(TypeRegistration), LifeStyleType.Singleton);
            Assert.That(systemUnderTest.LifeStyle, Is.EqualTo(LifeStyleType.Singleton));
        }
    }
}
