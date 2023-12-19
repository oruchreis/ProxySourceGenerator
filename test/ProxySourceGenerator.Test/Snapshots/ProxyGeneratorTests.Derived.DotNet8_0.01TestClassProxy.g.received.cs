//HintName: TestClassProxy.g.cs
using ProxySourceGenerator;
namespace Test
{
    partial interface ITestClass 
    {
        string AProperty { get; set; }
        void Method();
        int MethodReturnInt(string str);
        string ABaseMethod(int param1, long param2, List<int> param3);
    }

    internal static class TestClassProxyInitializer
    {
        [System.Runtime.CompilerServices.ModuleInitializerAttribute]
        public static void RegisterProxy()
        {
            ProxyAccessor<ITestClass>.Register(underlyingObject => new TestClassProxy(underlyingObject));
        }
    }
    
    partial class TestClassProxy: ITestClass, IGeneratedProxy<ITestClass> 
    {
        /// <inheritdoc/>
        public InterceptPropertyGetterHandler InterceptPropertyGetter { get; set; }
        /// <inheritdoc/>
        public InterceptPropertySetterHandler InterceptPropertySetter { get; set; }
        /// <inheritdoc/>
        public InterceptMethodHandler InterceptMethod { get; set; }
        /// <inheritdoc/>
        public ITestClass UnderlyingObject { get; set; }
        /// <inheritdoc/>
        ITestClass IGeneratedProxy<ITestClass>.Access => (ITestClass) this;
        public TestClassProxy (ITestClass underlyingObject)
            :base()
        {
            UnderlyingObject = underlyingObject;
        }
        #region public string AProperty Property
        protected virtual string OnGetAProperty(Func<string> getter)
        {
            return getter();
        }
        protected virtual void OnSetAProperty(Action<string> setter, string value)
        {
            setter(value);
        }
        public string AProperty
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
        #endregion //public string AProperty Property
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
        #region public int MethodReturnInt(string str) Method
        protected virtual int OnMethodReturnInt(Func<string, int> baseMethod, string str)
            
        {
            return baseMethod(str);
        }
        public int MethodReturnInt(string str)
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
        #endregion //public int MethodReturnInt(string str) Method
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
