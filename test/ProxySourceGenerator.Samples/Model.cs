using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxySourceGenerator.Samples;

public class Model: BaseModel, IModel
{
    public override T GenericMethod<T>()
    {
        return Activator.CreateInstance<T>();
    }
}

public partial class ModelProxy
{

}

public partial interface IModel
{

}