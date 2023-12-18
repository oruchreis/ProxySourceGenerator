//HintName: TestClassProxy.g.cs
using ProxySourceGenerator;
namespace Test
{
    partial interface ITestClass 
    {
        string AProperty { get; set; }
        string APropertyWithBody { get; set; }
        string APropertyWithExp { get; }
        string APropertyWithExp2 { get; set; }
        string APropertyOnlyGetter { get; }
        string APropertyOnlySetter { set; }
        string APropertyOnlyPrivateSetter { get; }
        void Method();
        int MethodReturnInt(string str);
    }

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
        #region public string APropertyWithBody Property
        protected virtual string OnGetAPropertyWithBody(Func<string> getter)
        {
            return getter();
        }
        protected virtual void OnSetAPropertyWithBody(Action<string> setter, string value)
        {
            setter(value);
        }
        public string APropertyWithBody
        {
            get 
            {
                if (InterceptPropertyGetter != null)
                    return (string)InterceptPropertyGetter("APropertyWithBody", () => OnGetAPropertyWithBody(() => UnderlyingObject.APropertyWithBody));
                else
                    return OnGetAPropertyWithBody(() => UnderlyingObject.APropertyWithBody);
            }
            set
            {
                if (InterceptPropertySetter != null)
                    InterceptPropertySetter("APropertyWithBody", value => OnSetAPropertyWithBody(v => UnderlyingObject.APropertyWithBody = v, (string)value), value);
                else
                    OnSetAPropertyWithBody(v => UnderlyingObject.APropertyWithBody = v, value);
            }
        }
        #endregion //public string APropertyWithBody Property
        #region public string APropertyWithExp Property
        protected virtual string OnGetAPropertyWithExp(Func<string> getter)
        {
            return getter();
        }
        public string APropertyWithExp
        {
            get 
            {
                if (InterceptPropertyGetter != null)
                    return (string)InterceptPropertyGetter("APropertyWithExp", () => OnGetAPropertyWithExp(() => UnderlyingObject.APropertyWithExp));
                else
                    return OnGetAPropertyWithExp(() => UnderlyingObject.APropertyWithExp);
            }
        }
        #endregion //public string APropertyWithExp Property
        #region public string APropertyWithExp2 Property
        protected virtual string OnGetAPropertyWithExp2(Func<string> getter)
        {
            return getter();
        }
        protected virtual void OnSetAPropertyWithExp2(Action<string> setter, string value)
        {
            setter(value);
        }
        public string APropertyWithExp2
        {
            get 
            {
                if (InterceptPropertyGetter != null)
                    return (string)InterceptPropertyGetter("APropertyWithExp2", () => OnGetAPropertyWithExp2(() => UnderlyingObject.APropertyWithExp2));
                else
                    return OnGetAPropertyWithExp2(() => UnderlyingObject.APropertyWithExp2);
            }
            set
            {
                if (InterceptPropertySetter != null)
                    InterceptPropertySetter("APropertyWithExp2", value => OnSetAPropertyWithExp2(v => UnderlyingObject.APropertyWithExp2 = v, (string)value), value);
                else
                    OnSetAPropertyWithExp2(v => UnderlyingObject.APropertyWithExp2 = v, value);
            }
        }
        #endregion //public string APropertyWithExp2 Property
        #region public string APropertyOnlyGetter Property
        protected virtual string OnGetAPropertyOnlyGetter(Func<string> getter)
        {
            return getter();
        }
        public string APropertyOnlyGetter
        {
            get 
            {
                if (InterceptPropertyGetter != null)
                    return (string)InterceptPropertyGetter("APropertyOnlyGetter", () => OnGetAPropertyOnlyGetter(() => UnderlyingObject.APropertyOnlyGetter));
                else
                    return OnGetAPropertyOnlyGetter(() => UnderlyingObject.APropertyOnlyGetter);
            }
        }
        #endregion //public string APropertyOnlyGetter Property
        #region public string APropertyOnlySetter Property
        protected virtual void OnSetAPropertyOnlySetter(Action<string> setter, string value)
        {
            setter(value);
        }
        public string APropertyOnlySetter
        {
            set
            {
                if (InterceptPropertySetter != null)
                    InterceptPropertySetter("APropertyOnlySetter", value => OnSetAPropertyOnlySetter(v => UnderlyingObject.APropertyOnlySetter = v, (string)value), value);
                else
                    OnSetAPropertyOnlySetter(v => UnderlyingObject.APropertyOnlySetter = v, value);
            }
        }
        #endregion //public string APropertyOnlySetter Property
        #region public string APropertyOnlyPrivateSetter Property
        protected virtual string OnGetAPropertyOnlyPrivateSetter(Func<string> getter)
        {
            return getter();
        }
        public string APropertyOnlyPrivateSetter
        {
            get 
            {
                if (InterceptPropertyGetter != null)
                    return (string)InterceptPropertyGetter("APropertyOnlyPrivateSetter", () => OnGetAPropertyOnlyPrivateSetter(() => UnderlyingObject.APropertyOnlyPrivateSetter));
                else
                    return OnGetAPropertyOnlyPrivateSetter(() => UnderlyingObject.APropertyOnlyPrivateSetter);
            }
        }
        #endregion //public string APropertyOnlyPrivateSetter Property
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
    }
}
