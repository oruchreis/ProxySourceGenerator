using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProxySourceGenerator.Samples;

[GenerateProxy(GenerateForDerived = true, AutoGenerateProxyInterface = true, UseInterface = true)]
public abstract class BaseModel
{
    public string StringProperty { get; set; }
    public int IntProperty { get; set; }
    public void ActionMethod(string strProp, int intProp)
    {
        StringProperty = strProp;
        IntProperty = intProp;
    }

    public int Multiply(int times)
        => IntProperty * times;

    public abstract T GenericMethod<T>();
}



public interface IModuleContext
{
}

public interface IModule
{
    int Prop1 { get; }

    IModuleContext Context { get; }

    string Prop2 { get; }

    Task MethodAsync();
}


[GenerateProxy(GenerateForDerived = true)]
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

    public virtual async Task MethodAsync()
    {
        await Task.Delay(1);
    }
}