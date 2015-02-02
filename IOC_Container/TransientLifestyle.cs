namespace IOC_Container
{
    class TransientLifestyle : ILifeStyle
    {
        public LifeStyleType Type { get { return LifeStyleType.Transient; } }

        public TInterface Resolve<TInterface>(TypeResolverContext context)
        {
            return TypeResolver.Resolve<TInterface>(context);
        }
    }
}
