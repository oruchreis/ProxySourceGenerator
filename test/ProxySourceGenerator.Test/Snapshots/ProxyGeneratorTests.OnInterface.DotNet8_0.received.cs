//HintName: TestClassProxy.g.cs
using ProxySourceGenerator;
namespace Test
{
    internal static class TestClassProxyInitializer
    {
        [System.Runtime.CompilerServices.ModuleInitializerAttribute]
        public static void RegisterProxy()
        {
            ProxySourceGenerator.ProxyAccessor<ITestClass>.Register(underlyingObject => new TestClassProxy(underlyingObject));
        }
    }
    
    partial class TestClassProxy: ITestClass, IGeneratedProxy<ITestClass> 
    {
        /// <inheritdoc/>
        public ProxySourceGenerator.InterceptPropertyGetterHandler InterceptPropertyGetter { get; set; }
        /// <inheritdoc/>
        public ProxySourceGenerator.InterceptPropertySetterHandler InterceptPropertySetter { get; set; }
        /// <inheritdoc/>
        public ProxySourceGenerator.InterceptMethodHandler InterceptMethod { get; set; }
        /// <inheritdoc/>
        public ITestClass UnderlyingObject { get; set; }
        /// <inheritdoc/>
        ITestClass IGeneratedProxy<ITestClass>.Access => (ITestClass) this;
        #region string AProperty Property
        protected virtual string OnGetAProperty(Func<string> getter)
        {
            return getter();
        }
        protected virtual void OnSetAProperty(Action<string> setter, string value)
        {
            setter(value);
        }
        string AProperty
        {
            get 
            {
                if (InterceptPropertyGetter != null)
                    return (string)InterceptPropertyGetter("AProperty", () => OnGetAProperty(() => UnderlyingObject.AProperty));
                else
                    return OnGetAProperty(() => UnderlyingObject.AProperty);
            }
            set
            {
                if (InterceptPropertySetter != null)
                    InterceptPropertySetter("AProperty", value => OnSetAProperty(v => UnderlyingObject.AProperty = v, (string)value), value);
                else
                    OnSetAProperty(v => UnderlyingObject.AProperty = v, value);
            }
        }
        #endregion //string AProperty Property
        #region public void Method() Method
        protected virtual void OnMethod(Action baseMethod)
            
        {
            baseMethod();
        }
        public void Method()
        {
            if (InterceptMethod != null)
                InterceptMethod(
                    "Method", 
                    p => {OnMethod(UnderlyingObject.Method); return null;},
                    new Dictionary<string, object> {
                        
                    }
                    );
            else
                OnMethod(UnderlyingObject.Method);
        }
        #endregion //public void Method() Method
        #region internal int MethodReturnInt(string str) Method
        protected virtual int OnMethodReturnInt(Func<string, int> baseMethod, string str)
            
        {
            return baseMethod(str);
        }
        internal int MethodReturnInt(string str)
        {
            if (InterceptMethod != null)
                return (int)InterceptMethod(
                    "MethodReturnInt", 
                    p => OnMethodReturnInt(UnderlyingObject.MethodReturnInt, (string)p["str"]),
                    new Dictionary<string, object> {
                        ["str"] = str
                    }
                    );
            else
                return OnMethodReturnInt(UnderlyingObject.MethodReturnInt, str);
        }
        #endregion //internal int MethodReturnInt(string str) Method
        #region protected string AProtectedMethod(int param1, long param2, List<int> param3) Method
        protected virtual string OnAProtectedMethod(Func<int, long, List<int>, string> baseMethod, int param1, long param2, List<int> param3)
            
        {
            return baseMethod(param1, param2, param3);
        }
        protected string AProtectedMethod(int param1, long param2, List<int> param3)
        {
            if (InterceptMethod != null)
                return (string)InterceptMethod(
                    "AProtectedMethod", 
                    p => OnAProtectedMethod(UnderlyingObject.AProtectedMethod, (int)p["param1"], (long)p["param2"], (List<int>)p["param3"]),
                    new Dictionary<string, object> {
                        ["param1"] = param1,
["param2"] = param2,
["param3"] = param3
                    }
                    );
            else
                return OnAProtectedMethod(UnderlyingObject.AProtectedMethod, param1, param2, param3);
        }
        #endregion //protected string AProtectedMethod(int param1, long param2, List<int> param3) Method
    }
}
