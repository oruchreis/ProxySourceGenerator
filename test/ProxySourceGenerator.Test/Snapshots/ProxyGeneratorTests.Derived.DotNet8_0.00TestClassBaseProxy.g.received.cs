//HintName: TestClassBaseProxy.g.cs
using ProxySourceGenerator;
namespace Test
{
    partial interface ITestClassBase 
    {
        string ABaseMethod(int param1, long param2, List<int> param3);
    }

    internal static class TestClassBaseProxyInitializer
    {
        [System.Runtime.CompilerServices.ModuleInitializerAttribute]
        public static void RegisterProxy()
        {
            ProxySourceGenerator.ProxyAccessor<ITestClassBase>.Register(underlyingObject => new TestClassBaseProxy(underlyingObject));
        }
    }
    
    partial class TestClassBaseProxy: ITestClassBase, IGeneratedProxy<ITestClassBase> 
    {
        /// <inheritdoc/>
        public ProxySourceGenerator.InterceptPropertyGetterHandler InterceptPropertyGetter { get; set; }
        /// <inheritdoc/>
        public ProxySourceGenerator.InterceptPropertySetterHandler InterceptPropertySetter { get; set; }
        /// <inheritdoc/>
        public ProxySourceGenerator.InterceptMethodHandler InterceptMethod { get; set; }
        /// <inheritdoc/>
        public ITestClassBase UnderlyingObject { get; set; }
        /// <inheritdoc/>
        ITestClassBase IGeneratedProxy<ITestClassBase>.Access => (ITestClassBase) this;
        public TestClassBaseProxy (ITestClassBase underlyingObject)
            :base()
        {
            UnderlyingObject = underlyingObject;
        }
        #region protected string ABaseMethod(int param1, long param2, List<int> param3) Method
        protected virtual string OnABaseMethod(Func<int, long, List<int>, string> baseMethod, int param1, long param2, List<int> param3)
            
        {
            return baseMethod(param1, param2, param3);
        }
        protected string ABaseMethod(int param1, long param2, List<int> param3)
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
        #endregion //protected string ABaseMethod(int param1, long param2, List<int> param3) Method
    }
}
