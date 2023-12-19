//HintName: TestClassProxy.g.cs
using ProxySourceGenerator;
namespace Test
{
    partial interface ITestClass 
    {
        string AProperty { get; set; }
        void Method();
    }

    internal static class TestClassProxyInitializer
    {
        [System.Runtime.CompilerServices.ModuleInitializerAttribute]
        public static void RegisterProxy()
        {
            ProxyAccessor<ITestClass>.Register(underlyingObject => new TestClassProxy(underlyingObject));
        }
    }
    
    [JsonSerializable][System.Runtime.Serialization.DataContract(Namespace = "asd")]
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
        #region [System.Runtime.Serialization.DataMember(Order = 0)] [JsonProperty] public string AProperty Property
        protected virtual string OnGetAProperty(Func<string> getter)
        {
            return getter();
        }
        protected virtual void OnSetAProperty(Action<string> setter, string value)
        {
            setter(value);
        }
        [System.Runtime.Serialization.DataMember(Order = 0)] [JsonProperty] public string AProperty
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
        #endregion //[System.Runtime.Serialization.DataMember(Order = 0)] [JsonProperty] public string AProperty Property
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
    }
}
