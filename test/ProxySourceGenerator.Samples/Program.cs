
using ProxySourceGenerator;
using ProxySourceGenerator.Samples;
using System.Reflection;

var model = new Model();
var modelProxy = ProxyAccessor<IModel>.Create(model);

modelProxy.InterceptMethod = (string methodName,
    InterceptMethodCallerHandler method,
    Dictionary<string, object> parameters) =>
{
    Console.WriteLine("method called: " + methodName + string.Join(",",parameters.Select(kv => (kv.Key, kv.Value).ToString())));
    return method(parameters);
};

modelProxy.InterceptAsyncMethod = async (string methodName,
    InterceptAsyncMethodCallerHandler methodAsync,
    Dictionary<string, object> parameters) =>
{
    Console.WriteLine("async method called: " + methodName + string.Join(",", parameters.Select(kv => (kv.Key, kv.Value).ToString())));
    return await methodAsync(parameters);

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
await modelProxy.Access.Method1Async();
var r2 = await modelProxy.Access.Method2Async("123");
var r3 = await modelProxy.Access.Method3Async("123");

Console.WriteLine("r2: " + r2);
Console.WriteLine("r3: " + r3);