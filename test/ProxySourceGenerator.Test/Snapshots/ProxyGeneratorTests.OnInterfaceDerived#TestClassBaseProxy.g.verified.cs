//HintName: TestClassBaseProxy.g.cs
using ProxySourceGenerator;
using System.Threading.Tasks;
namespace Test
{
    internal static class TestClassBaseProxyInitializer
    {
        [System.Runtime.CompilerServices.ModuleInitializerAttribute]
        public static void RegisterProxy()
        {
            ProxyAccessor<ITestClassBase>.Register(underlyingObject => new TestClassBaseProxy(underlyingObject));
        }
    }
    
    
    partial class TestClassBaseProxy: ITestClassBase, IGeneratedProxy<ITestClassBase> 
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
        public ITestClassBase UnderlyingObject { get; set; }
        /// <inheritdoc/>
        ITestClassBase IGeneratedProxy<ITestClassBase>.Access => (ITestClassBase) this;

        public TestClassBaseProxy(ITestClassBase underlyingObject): base()
        {
            UnderlyingObject = underlyingObject;
        }
        #region public async Task BaseMethod() Method
        protected virtual System.Threading.Tasks.Task On_BaseMethod(Func<System.Threading.Tasks.Task> baseMethod)
            
        {
            return baseMethod();
        }
        public async Task BaseMethod()
        {
            if (InterceptAsyncMethod != null)
                await InterceptAsyncMethod(
                    "BaseMethod", 
                    async p => {await On_BaseMethod(UnderlyingObject.BaseMethod); return null;},
                    new Dictionary<string, object> {
                        
                    }
                    );
            else
                await On_BaseMethod(UnderlyingObject.BaseMethod);
        }
        #endregion //public async Task BaseMethod() Method
    }
}
