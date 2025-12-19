//HintName: TestClass2Proxy.g.cs
using ProxySourceGenerator;
namespace Test
{
    partial interface ITestClass2 
    {
    }

    internal static class TestClass2ProxyInitializer
    {
        [System.Runtime.CompilerServices.ModuleInitializerAttribute]
        public static void RegisterProxy()
        {
            ProxyAccessor<ITestClass2>.Register(underlyingObject => new TestClass2Proxy(underlyingObject));
        }
    }
    
    
    partial class TestClass2Proxy: ITestClass2, IGeneratedProxy<ITestClass2> 
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
        public ITestClass2 UnderlyingObject { get; set; }
        /// <inheritdoc/>
        ITestClass2 IGeneratedProxy<ITestClass2>.Access => (ITestClass2) this;

        public TestClass2Proxy(ITestClass2 underlyingObject): base()
        {
            UnderlyingObject = underlyingObject;
        }
    }
}
