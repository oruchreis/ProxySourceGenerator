using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System.Reflection;

namespace ProxySourceGenerator.Test;

[TestClass]
public class ProxyGeneratorTests : VerifyBase
{
    private Task VerifyProxyAsync(params string[] sources)
    {
        // Parse the provided string into a C# syntax tree
        var syntaxTrees = sources.Select(source => CSharpSyntaxTree.ParseText(source)).ToArray();

        // Create references for assemblies we require
        // We could add multiple references if required
        IEnumerable<PortableExecutableReference> references = new[]
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(Assembly.Load("netstandard").Location),
            MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location),
            MetadataReference.CreateFromFile(typeof(GenerateProxyAttribute).Assembly.Location),
        };

        // Create a Roslyn compilation for the syntax tree.
        var compilation = CSharpCompilation.Create(
            assemblyName: "Tests",
            syntaxTrees: syntaxTrees,
            references: references);

        //var diagnostics = compilation.GetDiagnostics();

        // Create an instance of our EnumGenerator incremental source generator
        var generator = new ProxySourceGenerator();

        // The GeneratorDriver is used to run our generator against a compilation
        var driver = CSharpGeneratorDriver.Create(generator)
            .RunGenerators(compilation);
        // Use verify to snapshot test the source generator output!        
        return Verify(driver)
            .UseDirectory("Snapshots");
    }


    [TestMethod]
    public async Task PublicNotDerived()
    {
        await VerifyProxyAsync("""
            using ProxySourceGenerator;            
            using System.Threading.Tasks;

            namespace Test;
            [GenerateProxyAttribute]
            public class TestClass
            {
                public string AProperty { get; set; }
                public string APropertyWithBody { 
                    get
                    {
                        return "";
                    }
                    set 
                    {
                    }
                }
                public string APropertyWithExp => "";
                public string APropertyWithExp2 {
                    get => "";
                    set => AProperty = value;
                }
                public string APropertyOnlyGetter { get; }
                public string APropertyOnlySetter { set; }
                public string APropertyWithDefaultValue { get; set; } = "Default";
                public string APropertyOnlyPrivateSetter { get; private set; }
                public async Task Method()
                {
                }

                public int MethodReturnInt(string str)
                {
                    return 1;
                }
            }
            """);
    }

    [TestMethod]
    public async Task Derived()
    {
        await VerifyProxyAsync("""
            using ProxySourceGenerator;            
            using System.Threading.Tasks;

            namespace Test;
            [GenerateProxy(GenerateForDerived=true)]
            public abstract class TestClassBase
            {
                protected virtual string ABaseMethod(int param1, 
                    long param2, 
                    List<int> param3)
                {
                    return nameof(ABaseMethod);
                }
            }
            
            public class TestClass: TestClassBase
            {
                public string AProperty {get;set;}
                public async Task Method()
                {
                }
            
                public int MethodReturnInt(string str)
                {
                    return 1;
                }

                protected override string ABaseMethod(
                    int param1, 
                    long param2, 
                    List<int> param3
                )
                {
                    return "";
                }
            }
            """);
    }

    [TestMethod]
    public async Task GenericClassAndMethods()
    {
        await VerifyProxyAsync("""
            using ProxySourceGenerator;
            using System.Threading.Tasks;

            namespace Test;
            [GenerateProxyAttribute]
            public class TestClass<T>
                where T: struct
            {
                public async Task Method<TMethod>()
                    where TMethod: new()
                {
                }
            }
            """);
    }

    [TestMethod]
    public async Task DontUseInterface()
    {
        await VerifyProxyAsync("""
            using ProxySourceGenerator;
            using System.Threading.Tasks;

            namespace Test;
            [GenerateProxy(GenerateForDerived=true, UseInterface=false)]
            public abstract class TestClassBase
            {
                protected virtual string ABaseMethod(int param1, 
                    long param2, 
                    List<int> param3)
                {
                    return nameof(ABaseMethod);
                }
            }
            
            public class TestClass: TestClassBase
            {
                public virtual string AProperty {get;set;}
                public virtual void Method()
                {
                }
            
                internal virtual int MethodReturnInt(string str)
                {
                    return 1;
                }

                protected override string ABaseMethod(
                    int param1, 
                    long param2, 
                    List<int> param3
                )
                {
                    return "";
                }

                public async Task NotProxiedMethod(){}
            }
            """);
    }

    [TestMethod]
    public async Task OnInterface()
    {
        await VerifyProxyAsync("""
            using ProxySourceGenerator;
            using System.Threading.Tasks;

            namespace Test;
            [GenerateProxy]
            public interface ITestClass
            {
                string AProperty {get;set;}
                public async Task Method();
                internal int MethodReturnInt(string str);
                protected string AProtectedMethod(
                    int param1, 
                    long param2, 
                    List<int> param3
                );
            }
            
            public class TestClass: TestClassBase
            {
                public string AProperty {get;set;}
                public async Task Method()
                {
                }
            
                internal int MethodReturnInt(string str)
                {
                    return 1;
                }

                protected string AProtectedMethod(
                    int param1, 
                    long param2, 
                    List<int> param3
                )
                {
                    return "";
                }

                public async Task NotProxiedMethod(){}
            }
            """);
    }

    [TestMethod]
    public async Task OnInterfaceDerived()
    {
        await VerifyProxyAsync("""
            using ProxySourceGenerator;
            using System.Threading.Tasks;

            namespace Test;
            [GenerateProxy(GenerateForDerived=true)]
            public interface ITestClassBase
            {
                public async Task BaseMethod();
            }

            public interface ITestClass: ITestClassBase
            {
                string AProperty {get;set;}
                public async Task Method();
                internal int MethodReturnInt(string str);
                protected string AProtectedMethod(
                    int param1, 
                    long param2, 
                    List<int> param3
                );
            }
            
            
            """);
    }

    [TestMethod]
    public async Task KeepAttributes()
    {
        await VerifyProxyAsync("""
            using ProxySourceGenerator;
            using System.Threading.Tasks;

            namespace Test;
            [GenerateProxyAttribute]
            [JsonSerializable]
            [System.Runtime.Serialization.DataContract(Namespace = "asd")]
            public class TestClass
            {
                [System.Runtime.Serialization.DataMember(Order = 0)]
                [JsonProperty]
                public string AProperty { get; set; }                

                [System.Runtime.Serialization.IgnoreDataMember]
                [JsonIgnore]
                public async Task Method()
                {
                }
            }
            """);
    }

    [TestMethod]
    public async Task AsyncMethod()
    {
        await VerifyProxyAsync("""
            using ProxySourceGenerator;
            using System.Threading.Tasks;

            namespace Test;
            [GenerateProxyAttribute]
            public class TestClass
            {
                public async Task<bool> Method1Async(string param1, int param2)
                {
                    return true;
                }

                public Task<int> Method2Async(string param1, int param2)
                {
                    return Task.FromResult(0);
                }

                public Task Method3Async()
                {
                    return Task.CompletedTask;
                }
            }
            """);
    }

    [TestMethod]
    public async Task BaseClassWithCtorParameters()
    {
        await VerifyProxyAsync("""            
            using ProxySourceGenerator;

            namespace Test;
            [GenerateProxy(GenerateForDerived=true, UseInterface=false)]
            public class BaseClassWithUseInterfaceFalse
            {
                public BaseClass(string param1, int param2)
                {
                }
            }   
            
            public class TestClass1: BaseClassWithUseInterfaceFalse
            {
                public TestClass1(string param1, int param2)
                    : base(param1, param2)
                {
                }
            }

            [GenerateProxy(GenerateForDerived=true, UseInterface=true)]
            public class BaseClassWithUseInterfaceTrue
            {
                public BaseClassWithUseInterfaceTrue(string param1, int param2)
                {
                }
            }
            public class TestClass2: BaseClassWithUseInterfaceTrue
            {
                public TestClass2(string param1, int param2)
                    : base(param1, param2)
                {
                }
            }            
            """);

    }

    [TestMethod]
    public async Task VirtualOverrideMethods()
    {
        await VerifyProxyAsync("""            
            using ProxySourceGenerator;
            using System.Threading.Tasks;

            namespace Test;
            [GenerateProxy(GenerateForDerived=true)]
            public abstract class ModuleBase : IModule
            {
                protected ModuleBase(IModuleContext context)
                {
                    Context = context;
                }

                public IModuleContext Context { get; }
                public int Prop1 => GetProp1();

                public string Prop2 => "";

                protected virtual int GetProp1()
                {
                    return 1;
                }

                public virtual Task MethodAsync()
                {
                    return Task.CompletedTask;
                }
            }
            
            public interface IModule
            {
                int Prop1 { get; }

                IModuleContext Context { get; }

                string Prop2 { get; }

                Task MethodAsync();
            }

            public interface IModuleContext
            {
            }

            public class Module1 : ModuleBase, IModule1
            {
                public Module1(IModuleContext context) : base(context)
                {
                }

                public override async Task MethodAsync()
                {
                    await Task.Delay(1);
                }
            }
            """);

    }
}