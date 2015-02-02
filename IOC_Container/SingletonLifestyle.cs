using System;
using System.Collections.Generic;

namespace IOC_Container
{
    class SingletonLifestyle : ILifeStyle
    {
        public LifeStyleType Type { get { return LifeStyleType.Singleton; } }

        private IDictionary<Type, object> _instanceMap;
        private IDictionary<Type, object> InstanceMap
        {
            get
            {
                if (null == _instanceMap)
                {
                    InitializeInstanceMap(new Dictionary<Type, object>());
                }
                return _instanceMap;
            }
        }
       
        /// <summary>
        /// Initializes the instance map used by the life style. 
        /// </summary>
        /// <param name="typeRegistry">The dictionary to be used to keep track of instances of interface types.</param>
        protected void InitializeInstanceMap(IDictionary<Type, object> instanceMap) 
        {
            if (null == instanceMap)
                throw new ArgumentNullException("instanceMap");
            if (null != _instanceMap)
                throw new InvalidOperationException("InstanceMap is already initialized.");
            _instanceMap = instanceMap;
        }
     
        public TInterface Resolve<TInterface>(TypeResolverContext context)
        {
            Type key = typeof(TInterface);

            object instance = InstanceMap.ContainsKey(key) ? InstanceMap[key] : null; 

            if (null == instance)
            {
                instance = TypeResolver.Resolve<TInterface>(context);
                InstanceMap.Add(typeof(TInterface), instance);
            }

            return (TInterface)instance;
        }
    }
}
