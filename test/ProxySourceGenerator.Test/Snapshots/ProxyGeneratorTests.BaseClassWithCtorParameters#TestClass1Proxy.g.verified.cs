//HintName: TestClass1Proxy.g.cs
using ProxySourceGenerator;
namespace Test
{
    internal static class TestClass1ProxyInitializer
    {
        [System.Runtime.CompilerServices.ModuleInitializerAttribute]
        public static void RegisterProxy()
        {
            ProxyAccessor<TestClass1>.Register(underlyingObject => new TestClass1Proxy(underlyingObject));
        }
    }
    
    
    partial class TestClass1Proxy: TestClass1, IGeneratedProxy<TestClass1> 
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
        public TestClass1 UnderlyingObject { get; set; }
        /// <inheritdoc/>
        TestClass1 IGeneratedProxy<TestClass1>.Access => (TestClass1) this;

        public TestClass1Proxy(TestClass1 underlyingObject): base()
        {
            UnderlyingObject = underlyingObject;
        }
        public TestClass1Proxy (TestClass1 underlyingObject, string param1, int param2)
            :base(param1, param2)
        {
            UnderlyingObject = underlyingObject;
        }
    }
}
