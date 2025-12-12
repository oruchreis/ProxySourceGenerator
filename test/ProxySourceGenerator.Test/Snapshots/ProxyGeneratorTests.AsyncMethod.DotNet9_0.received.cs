//HintName: TestClassProxy.g.cs
using ProxySourceGenerator;
using System.Threading.Tasks;
namespace Test
{
    partial interface ITestClass 
    {
        Task<bool> Method1Async(string param1, int param2);
        Task<int> Method2Async(string param1, int param2);
        Task Method3Async();
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
        protected virtual System.Threading.Tasks.Task<bool> OnMethod1Async(Func<string, int, System.Threading.Tasks.Task<bool>> baseMethod, string param1, int param2)
            
        {
            return baseMethod(param1, param2);
        }
        public async Task<bool> Method1Async(string param1, int param2)
        {
            if (InterceptAsyncMethod != null)
                return await ((System.Threading.Tasks.Task<bool>)InterceptAsyncMethod(
                    "Method1Async", 
                    p => OnMethod1Async(UnderlyingObject.Method1Async, (string)p["param1"], (int)p["param2"]),
                    new Dictionary<string, object> {
                        ["param1"] = param1,
["param2"] = param2
                    }
                    ));
            else
                return OnMethod1Async(UnderlyingObject.Method1Async, param1, param2);
        }
        #endregion //public async Task<bool> Method1Async(string param1, int param2) Method
        #region public async Task<int> Method2Async(string param1, int param2) Method
        protected virtual System.Threading.Tasks.Task<int> OnMethod2Async(Func<string, int, System.Threading.Tasks.Task<int>> baseMethod, string param1, int param2)
            
        {
            return baseMethod(param1, param2);
        }
        public async Task<int> Method2Async(string param1, int param2)
        {
            if (InterceptAsyncMethod != null)
                return await ((System.Threading.Tasks.Task<int>)InterceptAsyncMethod(
                    "Method2Async", 
                    p => OnMethod2Async(UnderlyingObject.Method2Async, (string)p["param1"], (int)p["param2"]),
                    new Dictionary<string, object> {
                        ["param1"] = param1,
["param2"] = param2
                    }
                    ));
            else
                return OnMethod2Async(UnderlyingObject.Method2Async, param1, param2);
        }
        #endregion //public async Task<int> Method2Async(string param1, int param2) Method
        #region public async Task Method3Async() Method
        protected virtual System.Threading.Tasks.Task OnMethod3Async(Func<System.Threading.Tasks.Task> baseMethod)
            
        {
            return baseMethod();
        }
        public async Task Method3Async()
        {
            if (InterceptAsyncMethod != null)
                return await ((System.Threading.Tasks.Task)InterceptAsyncMethod(
                    "Method3Async", 
                    p => OnMethod3Async(UnderlyingObject.Method3Async),
                    new Dictionary<string, object> {
                        
                    }
                    ));
            else
                return OnMethod3Async(UnderlyingObject.Method3Async);
        }
        #endregion //public async Task Method3Async() Method
    }
}
