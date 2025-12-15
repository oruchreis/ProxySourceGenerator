using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxySourceGenerator.Samples;

public class Model : BaseModel, IModel
{
    public override T GenericMethod<T>()
    {
        return Activator.CreateInstance<T>();
    }

    public Task Method1Async()
    {
        return Task.CompletedTask;
    }

    public Task<int> Method2Async(string str)
    {
        return Task.FromResult(str.Length);
    }

    public async Task<string> Method3Async(string str)
    {
        return str.TrimEnd();
    }

    protected virtual void ProtectedMethod(string strProp, int intProp)
    {
        // Do something
    }
}

public partial class ModelProxy
{

}

public partial interface IModel
{

}