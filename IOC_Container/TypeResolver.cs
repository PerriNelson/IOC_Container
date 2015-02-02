using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace IOC_Container
{
    public static class TypeResolver
    {
        private class TypeConstructor
        {
            public ConstructorInfo Constructor { get; set; }
            public object[] Parameters { get; set; }
        }

        private static ILifeStyle[] _lifeStyles = new ILifeStyle[] { new TransientLifestyle(), new SingletonLifestyle() };

        public static TInterface Resolve<TInterface>(TypeResolverContext context)
        {
            ITypeRegistration registration = context.RegisteredTypes.FirstOrDefault(tr => tr.InterfaceType == typeof(TInterface));
            if (null == registration)
                throw new InvalidOperationException("Unregistered interfaces cannot be resolved to a concrete type.");

            TypeConstructor typeConstructor = ResolveDependencies(context, registration);
            return (TInterface)typeConstructor.Constructor.Invoke(typeConstructor.Parameters);
        }

        public static TInterface ResolveWithLifeStyle<TInterface>(TypeResolverContext context, LifeStyleType lifeStyleType)
        {
            ILifeStyle lifeStyle = _lifeStyles.FirstOrDefault(ls => ls.Type == lifeStyleType);
            if (null == lifeStyle)
                throw new InvalidOperationException("Unknown lifestyle type");

            return lifeStyle.Resolve<TInterface>(context);
        }

        private static object Resolve(Type type, TypeResolverContext context, LifeStyleType lifeStyleType)
        {
            MethodInfo method = typeof(TypeResolver).GetMethod("ResolveWithLifeStyle");
            MethodInfo generic = method.MakeGenericMethod(type);
            return generic.Invoke(null, new object[] { context, lifeStyleType });
        }

        private static TypeConstructor ResolveDependencies(TypeResolverContext context, ITypeRegistration registration)
        {
            ConstructorInfo defaultConstructor = null;

            foreach (ConstructorInfo constructor in registration.ConcreteType.GetConstructors(BindingFlags.Instance |
                BindingFlags.Public))
            {
                IList<ParameterInfo> parameters = constructor.GetParameters();
                if (parameters.Count == 0)
                {
                    defaultConstructor = constructor;
                    continue;
                }

                if (!parameters.All(p => context.RegisteredTypes.Any(r => r.InterfaceType == p.ParameterType)))
                    continue;

                List<object> resolvedDependencies = new List<object>(parameters.Count);
                foreach (ParameterInfo parameter in parameters)
                {
                    ITypeRegistration parameterType = context.RegisteredTypes.First(r => r.InterfaceType == parameter.ParameterType);

                    resolvedDependencies.Add(Resolve(parameterType.InterfaceType, context, parameterType.LifeStyle));
                }
                return new TypeConstructor()
                {
                    Constructor = constructor,
                    Parameters = resolvedDependencies.ToArray()
                };
            }
            return new TypeConstructor()
            {
                Constructor = defaultConstructor,
                Parameters = null
            };
        }
    }
}
