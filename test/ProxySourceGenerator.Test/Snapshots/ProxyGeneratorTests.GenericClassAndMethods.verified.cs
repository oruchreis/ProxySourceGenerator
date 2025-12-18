//HintName: TestClassProxy{T}.g.cs
using ProxySourceGenerator;
namespace Test
{
    partial interface ITestClass<T> where T: struct
    {
        void Method<TMethod>()
    where TMethod : new();
    }

    internal static class TestClassProxy<T>Initializer
    {
        [System.Runtime.CompilerServices.ModuleInitializerAttribute]
        public static void RegisterProxy()
        {
            ProxyAccessor<ITestClass<T>>.Register(underlyingObject => new TestClassProxy<T>(underlyingObject));
        }
    }
    
    
    partial class TestClassProxy<T>: ITestClass<T>, IGeneratedProxy<ITestClass<T>> where T: struct
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
        public ITestClass<T> UnderlyingObject { get; set; }
        /// <inheritdoc/>
        ITestClass<T> IGeneratedProxy<ITestClass<T>>.Access => (ITestClass<T>) this;

        public TestClassProxy<T>(ITestClass<T> underlyingObject): base()
        {
            UnderlyingObject = underlyingObject;
        }
        #region public void Method<TMethod>()
    where TMethod : new() Method
        protected virtual void On_Method<TMethod>(Action baseMethod)
            where TMethod : new()
        {
            baseMethod();
        }
        public void Method<TMethod>()
    where TMethod : new()
        {
            if (InterceptMethod != null)
                InterceptMethod(
                    "Method<TMethod>", 
                    p => {On_Method<TMethod>(UnderlyingObject.Method<TMethod>); return null;},
                    new Dictionary<string, object> {
                        
                    }
                    );
            else
                On_Method<TMethod>(UnderlyingObject.Method<TMethod>);
        }
        #endregion //public void Method<TMethod>()
    where TMethod : new() Method
    }
}
