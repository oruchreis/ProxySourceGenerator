//HintName: ModuleBaseProxy.g.cs
using ProxySourceGenerator;
using System.Threading.Tasks;
namespace Test
{
    partial interface IModuleBase 
    {
        IModuleContext Context { get; }
        int Prop1 { get; }
        string Prop2 { get; }
        Task MethodAsync();
    }

    internal static class ModuleBaseProxyInitializer
    {
        [System.Runtime.CompilerServices.ModuleInitializerAttribute]
        public static void RegisterProxy()
        {
            ProxyAccessor<IModuleBase>.Register(underlyingObject => new ModuleBaseProxy(underlyingObject));
        }
    }
    
    
    partial class ModuleBaseProxy: IModuleBase, IGeneratedProxy<IModuleBase> 
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
        public IModuleBase UnderlyingObject { get; set; }
        /// <inheritdoc/>
        IModuleBase IGeneratedProxy<IModuleBase>.Access => (IModuleBase) this;

        public ModuleBaseProxy(IModuleBase underlyingObject): base()
        {
            UnderlyingObject = underlyingObject;
        }
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
    }
}
