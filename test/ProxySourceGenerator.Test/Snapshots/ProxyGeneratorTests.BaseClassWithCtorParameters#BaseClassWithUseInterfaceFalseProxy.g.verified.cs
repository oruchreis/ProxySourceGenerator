//HintName: BaseClassWithUseInterfaceFalseProxy.g.cs
using ProxySourceGenerator;
namespace Test
{
    internal static class BaseClassWithUseInterfaceFalseProxyInitializer
    {
        [System.Runtime.CompilerServices.ModuleInitializerAttribute]
        public static void RegisterProxy()
        {
            ProxyAccessor<BaseClassWithUseInterfaceFalse>.Register(underlyingObject => new BaseClassWithUseInterfaceFalseProxy(underlyingObject));
        }
    }
    
    
    partial class BaseClassWithUseInterfaceFalseProxy: BaseClassWithUseInterfaceFalse, IGeneratedProxy<BaseClassWithUseInterfaceFalse> 
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
        public BaseClassWithUseInterfaceFalse UnderlyingObject { get; set; }
        /// <inheritdoc/>
        BaseClassWithUseInterfaceFalse IGeneratedProxy<BaseClassWithUseInterfaceFalse>.Access => (BaseClassWithUseInterfaceFalse) this;

        public BaseClassWithUseInterfaceFalseProxy(BaseClassWithUseInterfaceFalse underlyingObject): base()
        {
            UnderlyingObject = underlyingObject;
        }
        public BaseClassWithUseInterfaceFalseProxy (BaseClassWithUseInterfaceFalse underlyingObject, string param1, int param2)
            :base(param1, param2)
        {
            UnderlyingObject = underlyingObject;
        }
    }
}
