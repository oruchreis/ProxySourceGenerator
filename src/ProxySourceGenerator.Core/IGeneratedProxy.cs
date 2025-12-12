using System;
using System.Collections.Concurrent;

namespace ProxySourceGenerator;

/// <summary>
/// Represents generated proxy.
/// </summary>
/// <typeparam name="TUnderlyingType">The Underlying type which proxied</typeparam>
public interface IGeneratedProxy<TUnderlyingType>
{    
    /// <summary>
    /// Intercepts the <c>get</c> calls of all properties in the type, including base ones. 
    /// <para>Default: <c>(string propertyName, Func&lt;object&gt; getter) => getter();</c></para>
    /// </summary>
    public InterceptPropertyGetterHandler InterceptPropertyGetter { get; set; }
    /// <summary>
    /// Intercepts the <c>set</c> calls of all properties in the type, including base ones. 
    /// <para>Default: <c>(string propertyName, Action&lt;object&gt; setter, object value) => setter(value);</c></para>
    /// </summary>
    public InterceptPropertySetterHandler InterceptPropertySetter { get; set; }
    /// <summary>
    /// Intercepts the <c>method</c> calls of all properties in the type, including base ones. 
    /// <para>Default: <c>(string methodName, InterceptMethodCallerHandler method, Dictionary&lt;string, object&gt; parameters) => method(parameters);</c></para>
    /// </summary>
    public InterceptMethodHandler InterceptMethod { get; set; }

    /// <summary>
    /// Gets or sets the delegate that intercepts asynchronous method invocations.
    /// </summary>
    /// <remarks>Use this property to provide custom logic that is executed when an asynchronous method is
    /// called. This can be used for logging, validation, or modifying the behavior of asynchronous
    /// operations.</remarks>
    public InterceptAsyncMethodHandler InterceptAsyncMethod { get; set; }

    /// <summary>
    /// Returns the underlying object that proxied. This object is the original object reference that proxied.
    /// </summary>
    public TUnderlyingType UnderlyingObject { get; set; }

    /// <summary>
    /// Access members of <typeparamref name="TUnderlyingType"/>.
    /// </summary>
    TUnderlyingType Access { get; }
}

/// <summary>
/// Generic proxy accessor for <typeparamref name="TUnderlyingType"/>
/// </summary>
/// <typeparam name="TUnderlyingType"></typeparam>
public static class ProxyAccessor<TUnderlyingType>
{
    private static Func<TUnderlyingType, IGeneratedProxy<TUnderlyingType>> _constructor;    

    /// <summary>
    /// Using by module initilizers in the generated codes. Don't use directly.
    /// </summary>
    /// <param name="constructor"></param>
    public static void Register(Func<TUnderlyingType, IGeneratedProxy<TUnderlyingType>> constructor)
    {
        _constructor = constructor;
    }

    /// <summary>
    /// Creates a proxy object on top of <typeparamref name="TUnderlyingType"/>
    /// </summary>
    /// <param name="underlyingObject"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static IGeneratedProxy<TUnderlyingType> Create(TUnderlyingType underlyingObject)
    {
        if (_constructor == null)
            throw new ArgumentException("Couldn't find proxy class for the object '{0}'. Be sure the class is decorated with [GenerateProxy] attribute.", nameof(underlyingObject));
        return _constructor(underlyingObject);
    }
}