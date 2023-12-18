//HintName: TestClassProxy.g.cs
using ProxySourceGenerator;
using System.Collections.Generic;
using System;
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
        #region public void MethodProxied() Method
        protected virtual void OnMethodProxied(Action baseMethod)
            
        {
            baseMethod();
        }
        public void MethodProxied()
        {
            if (InterceptMethod != null)
                InterceptMethod(
                    "MethodProxied", 
                    p => {OnMethodProxied(UnderlyingObject.MethodProxied); return null;},
                    new Dictionary<string, object> {
                        
                    }
                    );
            else
                OnMethodProxied(UnderlyingObject.MethodProxied);
        }
        #endregion //public void MethodProxied() Method
    }
}
