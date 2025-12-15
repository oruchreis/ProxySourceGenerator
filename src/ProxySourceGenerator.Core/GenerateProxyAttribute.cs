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

    /// <summary>
    /// Represents the default naming pattern used for generated getter proxy methods.
    /// </summary>
    /// <remarks>This pattern is typically used when creating dynamic proxies for property getters. The "$1"
    /// placeholder is replaced with the property name at runtime or during code generation.</remarks>
    public const string DefaultGetterProxyNamePattern = "On_$1_Getter";
    /// <summary>
    /// Represents the default naming pattern used for setter proxy methods.
    /// </summary>
    /// <remarks>This pattern can be used to generate method names dynamically for proxies that intercept
    /// property setters. The "$1" placeholder is typically replaced with the property name.</remarks>
    public const string DefaultSetterProxyNamePattern = "On_$1_Setter";
    /// <summary>
    /// Represents the default pattern used to generate proxy method names.
    /// </summary>
    /// <remarks>The pattern uses "$1" as a placeholder for the original method name. This constant can be
    /// used when creating dynamic proxies or interceptors that require a consistent naming convention for generated
    /// methods.</remarks>
    public const string DefaultMethodProxyNamePattern = "On_$1";

    /// <summary>
    /// Gets or sets the pattern used to generate proxy method names for property getters.
    /// </summary>
    /// <remarks>The pattern may include placeholders, such as "$1", which will be replaced with the property
    /// name when generating the proxy method name.</remarks>
    public string GetterProxyNamePattern { get; set; } = DefaultGetterProxyNamePattern;

    /// <summary>
    /// Gets or sets the pattern used to generate proxy method names for property setters.
    /// </summary>
    /// <remarks>The pattern may include placeholders, such as "$1", which will be replaced with the
    /// corresponding property name when generating the proxy method name.</remarks>
    public string SetterProxyNamePattern { get; set; } = DefaultSetterProxyNamePattern;

    /// <summary>
    /// Gets or sets the pattern used to generate proxy method names.
    /// </summary>
    /// <remarks>The pattern may include placeholders, such as "$1", which will be replaced with the original
    /// method name or other relevant values when generating proxy methods. Adjust this pattern to control the naming
    /// convention of generated proxy methods.</remarks>
    public string MethodProxyNamePattern { get; set; } = DefaultMethodProxyNamePattern;
}

