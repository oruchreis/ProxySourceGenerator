//HintName: Module1Proxy.g.cs
using ProxySourceGenerator;
using System.Threading.Tasks;
namespace Test
{
    partial interface IModule1 
    {
        Task MethodAsync();
        IModuleContext Context { get; }
        int Prop1 { get; }
        string Prop2 { get; }
    }

    internal static class Module1ProxyInitializer
    {
        [System.Runtime.CompilerServices.ModuleInitializerAttribute]
        public static void RegisterProxy()
        {
            ProxyAccessor<IModule1>.Register(underlyingObject => new Module1Proxy(underlyingObject));
        }
    }
    
    
    partial class Module1Proxy: IModule1, IGeneratedProxy<IModule1> 
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
        public IModule1 UnderlyingObject { get; set; }
        /// <inheritdoc/>
        IModule1 IGeneratedProxy<IModule1>.Access => (IModule1) this;

        public Module1Proxy(IModule1 underlyingObject): base()
        {
            UnderlyingObject = underlyingObject;
        }
        #region public async Task MethodAsync() Method
        protected virtual System.Threading.Tasks.Task On_MethodAsync(Func<System.Threading.Tasks.Task> baseMethod)
            
        {
            return baseMethod();
        }
        public async Task MethodAsync()
        {
            if (InterceptAsyncMethod != null)
                await InterceptAsyncMethod(
                    "MethodAsync", 
                    async p => {await On_MethodAsync(UnderlyingObject.MethodAsync); return null;},
                    new Dictionary<string, object> {
                        
                    }
                    );
            else
                await On_MethodAsync(UnderlyingObject.MethodAsync);
        }
        #endregion //public async Task MethodAsync() Method
        #region public IModuleContext Context Property
        protected virtual Test.IModuleContext On_Context_Getter(Func<Test.IModuleContext> getter)
        {
            return getter();
        }
        public IModuleContext Context
        {
            get 
            {
                if (InterceptPropertyGetter != null)
                    return (Test.IModuleContext)InterceptPropertyGetter("Context", () => On_Context_Getter(() => UnderlyingObject.Context));
                else
                    return On_Context_Getter(() => UnderlyingObject.Context);
            }
        }
        #endregion //public IModuleContext Context Property
        #region public int Prop1 Property
        protected virtual int On_Prop1_Getter(Func<int> getter)
        {
            return getter();
        }
        public int Prop1
        {
            get 
            {
                if (InterceptPropertyGetter != null)
                    return (int)InterceptPropertyGetter("Prop1", () => On_Prop1_Getter(() => UnderlyingObject.Prop1));
                else
                    return On_Prop1_Getter(() => UnderlyingObject.Prop1);
            }
        }
        #endregion //public int Prop1 Property
        #region public string Prop2 Property
        protected virtual string On_Prop2_Getter(Func<string> getter)
        {
            return getter();
        }
        public string Prop2
        {
            get 
            {
                if (InterceptPropertyGetter != null)
                    return (string)InterceptPropertyGetter("Prop2", () => On_Prop2_Getter(() => UnderlyingObject.Prop2));
                else
                    return On_Prop2_Getter(() => UnderlyingObject.Prop2);
            }
        }
        #endregion //public string Prop2 Property
    }
}
