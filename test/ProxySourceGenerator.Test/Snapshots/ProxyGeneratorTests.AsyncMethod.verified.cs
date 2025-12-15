//HintName: TestClassProxy.g.cs
using ProxySourceGenerator;
using System.Threading.Tasks;
namespace Test
{
    partial interface ITestClass 
    {
        public Task<bool> Method1Async(string param1, int param2);
        public Task<int> Method2Async(string param1, int param2);
        public Task Method3Async();
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
        #region public async Task<bool> Method1Async(string param1, int param2) Method
        protected virtual System.Threading.Tasks.Task<bool> On_Method1Async(Func<string, int, System.Threading.Tasks.Task<bool>> baseMethod, string param1, int param2)
            
        {
            return baseMethod(param1, param2);
        }
        public async Task<bool> Method1Async(string param1, int param2)
        {
            if (InterceptAsyncMethod != null)
                return (bool)await InterceptAsyncMethod(
                    "Method1Async", 
                    async p => await On_Method1Async(UnderlyingObject.Method1Async, (string)p["param1"], (int)p["param2"]),
                    new Dictionary<string, object> {
                        ["param1"] = param1,
["param2"] = param2
                    }
                    );
            else
                return await On_Method1Async(UnderlyingObject.Method1Async, param1, param2);
        }
        #endregion //public async Task<bool> Method1Async(string param1, int param2) Method
        #region public async Task<int> Method2Async(string param1, int param2) Method
        protected virtual System.Threading.Tasks.Task<int> On_Method2Async(Func<string, int, System.Threading.Tasks.Task<int>> baseMethod, string param1, int param2)
            
        {
            return baseMethod(param1, param2);
        }
        public async Task<int> Method2Async(string param1, int param2)
        {
            if (InterceptAsyncMethod != null)
                return (int)await InterceptAsyncMethod(
                    "Method2Async", 
                    async p => await On_Method2Async(UnderlyingObject.Method2Async, (string)p["param1"], (int)p["param2"]),
                    new Dictionary<string, object> {
                        ["param1"] = param1,
["param2"] = param2
                    }
                    );
            else
                return await On_Method2Async(UnderlyingObject.Method2Async, param1, param2);
        }
        #endregion //public async Task<int> Method2Async(string param1, int param2) Method
        #region public async Task Method3Async() Method
        protected virtual System.Threading.Tasks.Task On_Method3Async(Func<System.Threading.Tasks.Task> baseMethod)
            
        {
            return baseMethod();
        }
        public async Task Method3Async()
        {
            if (InterceptAsyncMethod != null)
                await InterceptAsyncMethod(
                    "Method3Async", 
                    async p => {await On_Method3Async(UnderlyingObject.Method3Async); return null;},
                    new Dictionary<string, object> {
                        
                    }
                    );
            else
                await On_Method3Async(UnderlyingObject.Method3Async);
        }
        #endregion //public async Task Method3Async() Method
    }
}
