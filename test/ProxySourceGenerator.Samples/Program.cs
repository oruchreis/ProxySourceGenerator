
using ProxySourceGenerator;
using ProxySourceGenerator.Samples;

var model = new Model();
var modelProxy = ProxyAccessor<IModel>.Create(model);

modelProxy.InterceptMethod = (string methodName,
    InterceptMethodCallerHandler method,
    Dictionary<string, object> parameters) =>
{
    Console.WriteLine("method: " + methodName + string.Join(",",parameters.Select(kv => (kv.Key, kv.Value).ToString())));
    return method(parameters);
};

modelProxy.Access.GenericMethod<int>();
modelProxy.Access.ActionMethod("asd", 1);
modelProxy.Access.StringProperty = "def";