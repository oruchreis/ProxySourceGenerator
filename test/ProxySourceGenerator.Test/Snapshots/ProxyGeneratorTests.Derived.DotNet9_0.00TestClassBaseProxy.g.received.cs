//HintName: TestClassBaseProxy.g.cs
using ProxySourceGenerator;
namespace Test
{
    partial interface ITestClassBase 
    {
    }

    internal static class TestClassBaseProxyInitializer
    {
        [System.Runtime.CompilerServices.ModuleInitializerAttribute]
        public static void RegisterProxy()
        {
            ProxyAccessor<ITestClassBase>.Register(underlyingObject => new TestClassBaseProxy(underlyingObject));
        }
    }
    
    
    partial class TestClassBaseProxy: ITestClassBase, IGeneratedProxy<ITestClassBase> 
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
        public ITestClassBase UnderlyingObject { get; set; }
        /// <inheritdoc/>
        ITestClassBase IGeneratedProxy<ITestClassBase>.Access => (ITestClassBase) this;

        public TestClassBaseProxy(ITestClassBase underlyingObject): base()
        {
            UnderlyingObject = underlyingObject;
        }
    }
}
