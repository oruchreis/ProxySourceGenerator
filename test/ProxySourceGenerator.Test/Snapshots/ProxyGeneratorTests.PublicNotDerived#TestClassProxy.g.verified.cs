//HintName: TestClassProxy.g.cs
using ProxySourceGenerator;
using System.Threading.Tasks;
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
        string APropertyWithDefaultValue { get; set; }
        string APropertyOnlyPrivateSetter { get; }
        Task Method();
        int MethodReturnInt(string str);
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
        public InterceptAsyncMethodHandler InterceptAsyncMethod { get; set; }
        /// <inheritdoc/>
        public ITestClass UnderlyingObject { get; set; }
        /// <inheritdoc/>
        ITestClass IGeneratedProxy<ITestClass>.Access => (ITestClass) this;

        public TestClassProxy(ITestClass underlyingObject): base()
        {
            UnderlyingObject = underlyingObject;
        }
        #region public string AProperty Property
        protected virtual string On_AProperty_Getter(Func<string> getter)
        {
            return getter();
        }
        protected virtual void On_AProperty_Setter(Action<string> setter, string value)
        {
            setter(value);
        }
        public string AProperty
        {
            get 
            {
                if (InterceptPropertyGetter != null)
                    return (string)InterceptPropertyGetter("AProperty", () => On_AProperty_Getter(() => UnderlyingObject.AProperty));
                else
                    return On_AProperty_Getter(() => UnderlyingObject.AProperty);
            }
            set
            {
                if (InterceptPropertySetter != null)
                    InterceptPropertySetter("AProperty", value => On_AProperty_Setter(v => UnderlyingObject.AProperty = v, (string)value), value);
                else
                    On_AProperty_Setter(v => UnderlyingObject.AProperty = v, value);
            }
        }
        #endregion //public string AProperty Property
        #region public string APropertyWithBody Property
        protected virtual string On_APropertyWithBody_Getter(Func<string> getter)
        {
            return getter();
        }
        protected virtual void On_APropertyWithBody_Setter(Action<string> setter, string value)
        {
            setter(value);
        }
        public string APropertyWithBody
        {
            get 
            {
                if (InterceptPropertyGetter != null)
                    return (string)InterceptPropertyGetter("APropertyWithBody", () => On_APropertyWithBody_Getter(() => UnderlyingObject.APropertyWithBody));
                else
                    return On_APropertyWithBody_Getter(() => UnderlyingObject.APropertyWithBody);
            }
            set
            {
                if (InterceptPropertySetter != null)
                    InterceptPropertySetter("APropertyWithBody", value => On_APropertyWithBody_Setter(v => UnderlyingObject.APropertyWithBody = v, (string)value), value);
                else
                    On_APropertyWithBody_Setter(v => UnderlyingObject.APropertyWithBody = v, value);
            }
        }
        #endregion //public string APropertyWithBody Property
        #region public string APropertyWithExp Property
        protected virtual string On_APropertyWithExp_Getter(Func<string> getter)
        {
            return getter();
        }
        public string APropertyWithExp
        {
            get 
            {
                if (InterceptPropertyGetter != null)
                    return (string)InterceptPropertyGetter("APropertyWithExp", () => On_APropertyWithExp_Getter(() => UnderlyingObject.APropertyWithExp));
                else
                    return On_APropertyWithExp_Getter(() => UnderlyingObject.APropertyWithExp);
            }
        }
        #endregion //public string APropertyWithExp Property
        #region public string APropertyWithExp2 Property
        protected virtual string On_APropertyWithExp2_Getter(Func<string> getter)
        {
            return getter();
        }
        protected virtual void On_APropertyWithExp2_Setter(Action<string> setter, string value)
        {
            setter(value);
        }
        public string APropertyWithExp2
        {
            get 
            {
                if (InterceptPropertyGetter != null)
                    return (string)InterceptPropertyGetter("APropertyWithExp2", () => On_APropertyWithExp2_Getter(() => UnderlyingObject.APropertyWithExp2));
                else
                    return On_APropertyWithExp2_Getter(() => UnderlyingObject.APropertyWithExp2);
            }
            set
            {
                if (InterceptPropertySetter != null)
                    InterceptPropertySetter("APropertyWithExp2", value => On_APropertyWithExp2_Setter(v => UnderlyingObject.APropertyWithExp2 = v, (string)value), value);
                else
                    On_APropertyWithExp2_Setter(v => UnderlyingObject.APropertyWithExp2 = v, value);
            }
        }
        #endregion //public string APropertyWithExp2 Property
        #region public string APropertyOnlyGetter Property
        protected virtual string On_APropertyOnlyGetter_Getter(Func<string> getter)
        {
            return getter();
        }
        public string APropertyOnlyGetter
        {
            get 
            {
                if (InterceptPropertyGetter != null)
                    return (string)InterceptPropertyGetter("APropertyOnlyGetter", () => On_APropertyOnlyGetter_Getter(() => UnderlyingObject.APropertyOnlyGetter));
                else
                    return On_APropertyOnlyGetter_Getter(() => UnderlyingObject.APropertyOnlyGetter);
            }
        }
        #endregion //public string APropertyOnlyGetter Property
        #region public string APropertyOnlySetter Property
        protected virtual void On_APropertyOnlySetter_Setter(Action<string> setter, string value)
        {
            setter(value);
        }
        public string APropertyOnlySetter
        {
            set
            {
                if (InterceptPropertySetter != null)
                    InterceptPropertySetter("APropertyOnlySetter", value => On_APropertyOnlySetter_Setter(v => UnderlyingObject.APropertyOnlySetter = v, (string)value), value);
                else
                    On_APropertyOnlySetter_Setter(v => UnderlyingObject.APropertyOnlySetter = v, value);
            }
        }
        #endregion //public string APropertyOnlySetter Property
        #region public string APropertyWithDefaultValue Property
        protected virtual string On_APropertyWithDefaultValue_Getter(Func<string> getter)
        {
            return getter();
        }
        protected virtual void On_APropertyWithDefaultValue_Setter(Action<string> setter, string value)
        {
            setter(value);
        }
        public string APropertyWithDefaultValue
        {
            get 
            {
                if (InterceptPropertyGetter != null)
                    return (string)InterceptPropertyGetter("APropertyWithDefaultValue", () => On_APropertyWithDefaultValue_Getter(() => UnderlyingObject.APropertyWithDefaultValue));
                else
                    return On_APropertyWithDefaultValue_Getter(() => UnderlyingObject.APropertyWithDefaultValue);
            }
            set
            {
                if (InterceptPropertySetter != null)
                    InterceptPropertySetter("APropertyWithDefaultValue", value => On_APropertyWithDefaultValue_Setter(v => UnderlyingObject.APropertyWithDefaultValue = v, (string)value), value);
                else
                    On_APropertyWithDefaultValue_Setter(v => UnderlyingObject.APropertyWithDefaultValue = v, value);
            }
        }
        #endregion //public string APropertyWithDefaultValue Property
        #region public string APropertyOnlyPrivateSetter Property
        protected virtual string On_APropertyOnlyPrivateSetter_Getter(Func<string> getter)
        {
            return getter();
        }
        public string APropertyOnlyPrivateSetter
        {
            get 
            {
                if (InterceptPropertyGetter != null)
                    return (string)InterceptPropertyGetter("APropertyOnlyPrivateSetter", () => On_APropertyOnlyPrivateSetter_Getter(() => UnderlyingObject.APropertyOnlyPrivateSetter));
                else
                    return On_APropertyOnlyPrivateSetter_Getter(() => UnderlyingObject.APropertyOnlyPrivateSetter);
            }
        }
        #endregion //public string APropertyOnlyPrivateSetter Property
        #region public async Task Method() Method
        protected virtual System.Threading.Tasks.Task On_Method(Func<System.Threading.Tasks.Task> baseMethod)
            
        {
            return baseMethod();
        }
        public async Task Method()
        {
            if (InterceptAsyncMethod != null)
                await InterceptAsyncMethod(
                    "Method", 
                    async p => {await On_Method(UnderlyingObject.Method); return null;},
                    new Dictionary<string, object> {
                        
                    }
                    );
            else
                await On_Method(UnderlyingObject.Method);
        }
        #endregion //public async Task Method() Method
        #region public int MethodReturnInt(string str) Method
        protected virtual int On_MethodReturnInt(Func<string, int> baseMethod, string str)
            
        {
            return baseMethod(str);
        }
        public int MethodReturnInt(string str)
        {
            if (InterceptMethod != null)
                return (int)InterceptMethod(
                    "MethodReturnInt", 
                    p => On_MethodReturnInt(UnderlyingObject.MethodReturnInt, (string)p["str"]),
                    new Dictionary<string, object> {
                        ["str"] = str
                    }
                    );
            else
                return On_MethodReturnInt(UnderlyingObject.MethodReturnInt, str);
        }
        #endregion //public int MethodReturnInt(string str) Method
    }
}
