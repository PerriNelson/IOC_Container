using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using IOC_Container;

namespace IOC_Container_Tests
{
    [TestFixture]
    public class InversionOfControlContainerTest
    {
        private InversionOfControlContainer systemUnderTest;
        private IDictionary<Type, ITypeRegistration> typeRegistry;

        [SetUp]
        public void Setup()
        {
            typeRegistry = new Dictionary<Type, ITypeRegistration>();
            systemUnderTest = new TestingInversionOfControlContainer(typeRegistry);
        }

        [Test]
        public void test_Register_AddsAnEntryToTypeRegistry()
        {
            systemUnderTest.Register<IInterface2, Class2>();
            Assert.That(typeRegistry, Is.Not.Empty);
        }

        [Test]
        public void test_Register_WhenCalledWithoutLifeStyeType_UsesLifeStyleTypeTransientAsDefault()
        {
            systemUnderTest.Register<IInterface2, Class2>();
            Assert.That(typeRegistry.Any(kvp => kvp.Value.LifeStyle == LifeStyleType.Transient && kvp.Key == typeof(IInterface2)),
                Is.True);
        }

        [Test]
        public void test_Register_WhenCalledWithSpeficiedLifeStyleType_UsesSpecifiedLifeStyleType()
        {
            systemUnderTest.Register<IInterface2, Class2>(LifeStyleType.Singleton);
            Assert.That(typeRegistry.Any(kvp => kvp.Value.LifeStyle == LifeStyleType.Singleton && kvp.Key == typeof(IInterface2)),
                Is.True);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage =
            "A concrete type for this interface has already been registered.")]
        public void test_Register_WhenCalledTwiceWithSameInterface_ThrowsMeaningfulException()
        {
            systemUnderTest.Register<IInterface2, Class2>();
            systemUnderTest.Register<IInterface2, Class2>();
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage =
            "Unregistered interfaces cannot be resolved to a concrete type.")]
        public void test_Resolve_WhenCalledForUnregisteredInterface_ThrowsMeaningfulException()
        {
            IInterface2 result = systemUnderTest.Resolve<IInterface2>();
        }
        
        [Test]
        public void test_Resolve_WhenCalledForRegisteredInterface_ReturnsExpectedConcreteType()
        {
            systemUnderTest.Register<IInterface2, Class2>();
            IInterface2 result = systemUnderTest.Resolve<IInterface2>();
            Assert.That(result, Is.InstanceOf<Class2>());
        }

        [Test]
        public void test_Resolve_WhenCalledTwiceForInterfaceWithLifeStyleTypeTransient_ReturnsDifferentObjects()
        {
            systemUnderTest.Register<IInterface2, Class2>();
            IInterface2 result1 = systemUnderTest.Resolve<IInterface2>();
            IInterface2 result2 = systemUnderTest.Resolve<IInterface2>();

            Assert.That(result1 == result2, Is.False);
        }

        [Test]
        public void test_Resolve_WhenCalledTwiceForInterfaceWithLifeStyleTypeSingleton_ReturnsSameObject()
        {
            systemUnderTest.Register<IInterface2, Class2>(LifeStyleType.Singleton);
            IInterface2 result1 = systemUnderTest.Resolve<IInterface2>();
            IInterface2 result2 = systemUnderTest.Resolve<IInterface2>();

            Assert.That(result1 == result2, Is.True);
        }

        [Test]
        public void test_Resolve_WhenCalledForInterfaceResolvingToConcreteClassWithDependencies_ResolvesDependencies()
        {
            systemUnderTest.Register<IInterface1, Class1>();
            systemUnderTest.Register<IInterface2, Class2>();

            IInterface1 result = systemUnderTest.Resolve<IInterface1>();

            Assert.That(result.Interface2, Is.InstanceOf<Class2>());
        }

        [Test]
        public void test_Resolve_WhenCalledTwiceForInterfaceResolvingToConcreteClassWithSingletonDependencies_ResolvesDependenciesOnce()
        {
            systemUnderTest.Register<IInterface1, Class1>();
            systemUnderTest.Register<IInterface2, Class2>(LifeStyleType.Singleton);

            IInterface1 result1 = systemUnderTest.Resolve<IInterface1>();
            IInterface1 result2 = systemUnderTest.Resolve<IInterface1>();

            Assert.That(result1.Interface2 == result2.Interface2, Is.True);
        }

        [Test]
        public void test_Resolve_WhenCalledTwiceForInterfaceResolvingToConcreteClassWithTransientDependencies_ResolvesDependenciesTwice()
        {
            systemUnderTest.Register<IInterface1, Class1>();
            systemUnderTest.Register<IInterface2, Class2>(LifeStyleType.Transient);

            IInterface1 result1 = systemUnderTest.Resolve<IInterface1>();
            IInterface1 result2 = systemUnderTest.Resolve<IInterface1>();

            Assert.That(result1.Interface2 == result2.Interface2, Is.False);
        }
    }
}
