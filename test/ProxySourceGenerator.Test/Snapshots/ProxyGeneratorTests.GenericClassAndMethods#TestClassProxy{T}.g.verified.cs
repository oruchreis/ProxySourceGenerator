//HintName: TestClassProxy{T}.g.cs
using ProxySourceGenerator;
using System.Threading.Tasks;
namespace Test
{
    partial interface ITestClass<T> where T: struct
    {
        Task Method<TMethod>()
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
        #region public async Task Method<TMethod>()
    where TMethod : new() Method
        protected virtual System.Threading.Tasks.Task On_Method<TMethod>(Func<System.Threading.Tasks.Task> baseMethod)
            where TMethod : new()
        {
            return baseMethod();
        }
        public async Task Method<TMethod>()
    where TMethod : new()
        {
            if (InterceptAsyncMethod != null)
                await InterceptAsyncMethod(
                    "Method<TMethod>", 
                    async p => {await On_Method<TMethod>(UnderlyingObject.Method<TMethod>); return null;},
                    new Dictionary<string, object> {
                        
                    }
                    );
            else
                await On_Method<TMethod>(UnderlyingObject.Method<TMethod>);
        }
        #endregion //public async Task Method<TMethod>()
    where TMethod : new() Method
    }
}
