using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System.Reflection;

namespace ProxySourceGenerator.Test;

[TestClass]
public class ProxyGeneratorTests : VerifyBase
{
    private Task VerifyProxy(params string[] sources)
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
    public void PublicNotDerived()
    {
        VerifyProxy("""
            using ProxySourceGenerator;

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
                public void Method()
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
    public void Derived()
    {
        VerifyProxy("""
            using ProxySourceGenerator;

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
                public void Method()
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
    public void GenericClassAndMethods()
    {
        VerifyProxy("""
            using ProxySourceGenerator;

            namespace Test;
            [GenerateProxyAttribute]
            public class TestClass<T>
                where T: struct
            {
                public void Method<TMethod>()
                    where TMethod: new()
                {
                }
            }
            """);
    }

    [TestMethod]
    public void DontUseInterface()
    {
        VerifyProxy("""
            using ProxySourceGenerator;

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

                public void NotProxiedMethod(){}
            }
            """);
    }

    [TestMethod]
    public void OnInterface()
    {
        VerifyProxy("""
            using ProxySourceGenerator;

            namespace Test;
            [GenerateProxy]
            public interface ITestClass
            {
                string AProperty {get;set;}
                public void Method();
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
                public void Method()
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

                public void NotProxiedMethod(){}
            }
            """);
    }

    [TestMethod]
    public void OnInterfaceDerived()
    {
        VerifyProxy("""
            using ProxySourceGenerator;

            namespace Test;
            [GenerateProxy(GenerateForDerived=true)]
            public interface ITestClassBase
            {
                public void BaseMethod();
            }

            public interface ITestClass: ITestClassBase
            {
                string AProperty {get;set;}
                public void Method();
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
    public void KeepAttributes()
    {
        VerifyProxy("""
            using ProxySourceGenerator;

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
                public void Method()
                {
                }
            }
            """);
    }

    [TestMethod]
    public void AsyncMethod()
    {
        VerifyProxy("""
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
            }
            """);
    }
}