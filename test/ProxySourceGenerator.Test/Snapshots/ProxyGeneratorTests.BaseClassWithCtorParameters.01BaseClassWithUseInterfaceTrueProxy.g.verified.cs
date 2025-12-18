//HintName: BaseClassWithUseInterfaceTrueProxy.g.cs
using ProxySourceGenerator;
namespace Test
{
    partial interface IBaseClassWithUseInterfaceTrue 
    {
    }

    internal static class BaseClassWithUseInterfaceTrueProxyInitializer
    {
        [System.Runtime.CompilerServices.ModuleInitializerAttribute]
        public static void RegisterProxy()
        {
            ProxyAccessor<IBaseClassWithUseInterfaceTrue>.Register(underlyingObject => new BaseClassWithUseInterfaceTrueProxy(underlyingObject));
        }
    }
    
    
    partial class BaseClassWithUseInterfaceTrueProxy: IBaseClassWithUseInterfaceTrue, IGeneratedProxy<IBaseClassWithUseInterfaceTrue> 
    {
        /// <inheritdoc/>
        public InterceptPropertyGetterHandler InterceptPropertyGetter { get; set; }
        /// <inheritdoc/>
        public InterceptPropertySetterHandler InterceptPropertySetter { get; set; }
        /// <inheritdoc/>
        public InterceptMethodHandler InterceptMethod { get; set; }
        /// <inheritdoc/>
        public InterceptAsyncMethodHandler InterceptAsyncMethod { get; set; }
        /// <inheritdoc/>
        public IBaseClassWithUseInterfaceTrue UnderlyingObject { get; set; }
        /// <inheritdoc/>
        IBaseClassWithUseInterfaceTrue IGeneratedProxy<IBaseClassWithUseInterfaceTrue>.Access => (IBaseClassWithUseInterfaceTrue) this;

        public BaseClassWithUseInterfaceTrueProxy(IBaseClassWithUseInterfaceTrue underlyingObject): base()
        {
            UnderlyingObject = underlyingObject;
        }
    }
}
