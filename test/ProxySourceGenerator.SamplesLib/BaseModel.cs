using System;
using System.Collections.Generic;
using System.Linq;
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
