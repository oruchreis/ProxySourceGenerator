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
    /// </summary>
    public InterceptPropertyGetterHandler InterceptPropertyGetter { get; set; }
    /// <summary>
    /// Intercepts the <c>set</c> calls of all properties in the type, including base ones.
    /// </summary>
    public InterceptPropertySetterHandler InterceptPropertySetter { get; set; }
    /// <summary>
    /// Intercepts the <c>method</c> calls of all properties in the type, including base ones.
    /// string methodName,
    /// InterceptMethodCallerHandler method,
    /// Dictionary<string, object> parameters
    /// </summary>
    public InterceptMethodHandler InterceptMethod { get; set; }

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