//HintName: TestClassBaseProxy.g.cs
using ProxySourceGenerator;
namespace Test
{
    internal static class TestClassBaseProxyInitializer
    {
        [System.Runtime.CompilerServices.ModuleInitializerAttribute]
        public static void RegisterProxy()
        {
            ProxyAccessor<TestClassBase>.Register(underlyingObject => new TestClassBaseProxy(underlyingObject));
        }
    }
    
    partial class TestClassBaseProxy: TestClassBase, IGeneratedProxy<TestClassBase> 
    {
        /// <inheritdoc/>
        public InterceptPropertyGetterHandler InterceptPropertyGetter { get; set; }
        /// <inheritdoc/>
        public InterceptPropertySetterHandler InterceptPropertySetter { get; set; }
        /// <inheritdoc/>
        public InterceptMethodHandler InterceptMethod { get; set; }
        /// <inheritdoc/>
        public TestClassBase UnderlyingObject { get; set; }
        /// <inheritdoc/>
        TestClassBase IGeneratedProxy<TestClassBase>.Access => (TestClassBase) this;
        public TestClassBaseProxy (TestClassBase underlyingObject)
            :base()
        {
            UnderlyingObject = underlyingObject;
        }
        #region protected override string ABaseMethod(int param1, long param2, List<int> param3) Method
        protected virtual string OnABaseMethod(Func<int, long, List<int>, string> baseMethod, int param1, long param2, List<int> param3)
            
        {
            return baseMethod(param1, param2, param3);
        }
        protected override string ABaseMethod(int param1, long param2, List<int> param3)
        {
            if (InterceptMethod != null)
                return (string)InterceptMethod(
                    "ABaseMethod", 
                    p => OnABaseMethod(UnderlyingObject.ABaseMethod, (int)p["param1"], (long)p["param2"], (List<int>)p["param3"]),
                    new Dictionary<string, object> {
                        ["param1"] = param1,
["param2"] = param2,
["param3"] = param3
                    }
                    );
            else
                return OnABaseMethod(UnderlyingObject.ABaseMethod, param1, param2, param3);
        }
        #endregion //protected override string ABaseMethod(int param1, long param2, List<int> param3) Method
    }
}
