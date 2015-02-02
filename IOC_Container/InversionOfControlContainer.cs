using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace IOC_Container
{
    public class InversionOfControlContainer
    {
        private IDictionary<Type, ITypeRegistration> _typeRegistry;
        private IDictionary<Type, ITypeRegistration> TypeRegistry {
            get
            {
                if (null == _typeRegistry)
                {
                    InitializeTypeRegistry(new Dictionary<Type, ITypeRegistration>());
                }
                return _typeRegistry;
            }
        }
       
        /// <summary>
        /// Initializes the type registry used by the container. 
        /// </summary>
        /// <param name="typeRegistry">The dictionary to be used to keep track of interface types and the
        /// concrete types that implement them.</param>
        protected void InitializeTypeRegistry(IDictionary<Type, ITypeRegistration> typeRegistry) 
        {
            if (null == typeRegistry)
                throw new ArgumentNullException("typeRegistry");
            if (null != _typeRegistry)
                throw new InvalidOperationException("TypeRegistry is already initialized.");

            _typeRegistry = typeRegistry;
        }

        public void Register<TInterface, TClass>(LifeStyleType lifeStyle = LifeStyleType.Transient)
            where TInterface : class
            where TClass : TInterface
        {
            if (TypeRegistry.Any(kvp => kvp.Key == typeof(TInterface)))
                throw new ArgumentException("A concrete type for this interface has already been registered.");

            TypeRegistry.Add(typeof(TInterface), new TypeRegistration(typeof(TInterface), typeof(TClass), lifeStyle));
        }
        
        public TInterface Resolve<TInterface>()
            where TInterface : class
        {
            ITypeRegistration registration = TypeRegistry.Select(kvp => kvp.Value)
                .Where(tr => tr.InterfaceType == typeof(TInterface)).FirstOrDefault();
            if (null == registration)
                throw new InvalidOperationException("Unregistered interfaces cannot be resolved to a concrete type.");

            return (TInterface)TypeResolver.ResolveWithLifeStyle<TInterface>(new TypeResolverContext()
            {
                RegisteredTypes = (IList<ITypeRegistration>)TypeRegistry.Values.ToList()
            }, registration.LifeStyle);
        }
    }
}