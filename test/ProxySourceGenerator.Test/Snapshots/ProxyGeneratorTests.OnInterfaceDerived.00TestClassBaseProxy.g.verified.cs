//HintName: TestClassBaseProxy.g.cs
using ProxySourceGenerator;
using System.Collections.Generic;
using System;
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
        public ITestClassBase UnderlyingObject { get; set; }
        /// <inheritdoc/>
        ITestClassBase IGeneratedProxy<ITestClassBase>.Access => (ITestClassBase) this;

        public TestClassBaseProxy(ITestClassBase underlyingObject): base()
        {
            UnderlyingObject = underlyingObject;
        }
        #region public void BaseMethod() Method
        protected virtual void OnBaseMethod(Action baseMethod)
            
        {
            baseMethod();
        }
        public void BaseMethod()
        {
            if (InterceptMethod != null)
                InterceptMethod(
                    "BaseMethod", 
                    p => {OnBaseMethod(UnderlyingObject.BaseMethod); return null;},
                    new Dictionary<string, object> {
                        
                    }
                    );
            else
                OnBaseMethod(UnderlyingObject.BaseMethod);
        }
        #endregion //public void BaseMethod() Method
    }
}
