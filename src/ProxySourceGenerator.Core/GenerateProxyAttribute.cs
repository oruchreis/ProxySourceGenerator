using System;

namespace ProxySourceGenerator;

/// <summary>
/// Generates proxy class. 
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
public class GenerateProxyAttribute : Attribute
{
    /// <summary>
    /// Generates proxy classes for derived classes from this class. Default: false
    /// </summary>
    public bool GenerateForDerived { get; set; } = false;

    /// <summary>
    /// Auto generates proxy interface for this class. Default: true
    /// </summary>
    public bool AutoGenerateProxyInterface { get; set; } = true;

    /// <summary>
    /// Use an interface to proxy the class. This internace must be implemented by this class. If you don't want to use to implement interface, you can only proxy virtual methods and properties. Default: true
    /// </summary>
    public bool UseInterface { get; set; } = true;
}

