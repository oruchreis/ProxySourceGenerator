using System;

namespace ProxySourceGenerator;

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