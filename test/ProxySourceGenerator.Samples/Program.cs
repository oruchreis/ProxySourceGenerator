
using ProxySourceGenerator;
using ProxySourceGenerator.Samples;

var model = new Model();
var modelProxy = ProxyAccessor<IModel>.Create(model);

modelProxy.InterceptMethod = (string methodName,
    InterceptMethodCallerHandler method,
    Dictionary<string, object> parameters) =>
{
    Console.WriteLine("method called: " + methodName + string.Join(",",parameters.Select(kv => (kv.Key, kv.Value).ToString())));
    return method(parameters);
};

modelProxy.InterceptPropertySetter = (propertyName, setter, value) =>
{
    Console.WriteLine($"property set: {propertyName} with value '{value}'");
    setter(value);
};

modelProxy.InterceptPropertyGetter = (propertyName, getter) =>
{
    Console.WriteLine($"property get: {propertyName}");
    return getter();
};

modelProxy.Access.GenericMethod<int>();
modelProxy.Access.ActionMethod("asd", 1);
modelProxy.Access.StringProperty = "def";