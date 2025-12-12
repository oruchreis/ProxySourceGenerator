using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProxySourceGenerator;

/// <summary>
/// Delegate of intercepting the getter of a property
/// </summary>
/// <param name="propertyName"></param>
/// <param name="getter"></param>
/// <returns></returns>
public delegate object InterceptPropertyGetterHandler(string propertyName, Func<object> getter);
/// <summary>
/// Delegate of intercepting the setter of a property
/// </summary>
/// <param name="propertyName"></param>
/// <param name="setter"></param>
/// <param name="value"></param>
public delegate void InterceptPropertySetterHandler(string propertyName, Action<object> setter, object value);

/// <summary>
/// Represents a method handler that intercepts a call and processes a set of named parameters.
/// </summary>
/// <remarks>This delegate is typically used in scenarios where method calls need to be dynamically intercepted,
/// such as in proxy or aspect-oriented programming frameworks. The caller is responsible for ensuring that the
/// dictionary contains all required parameters for the target method.</remarks>
/// <param name="parameters">A dictionary containing parameter names and their corresponding values to be passed to the intercepted method. Keys
/// must be non-null strings representing parameter names; values may be null depending on the method's requirements.</param>
/// <returns>An object representing the result of the intercepted method call. The type and meaning of the result depend on the
/// specific method being intercepted.</returns>
public delegate object InterceptMethodCallerHandler(Dictionary<string, object> parameters);

/// <summary>
/// Represents an asynchronous callback that handles method interception by processing a set of named parameters.
/// </summary>
/// <remarks>This delegate is typically used in scenarios where method calls are intercepted and custom logic is
/// applied before, after, or instead of the original method execution. The returned object should match the expected
/// return type of the intercepted method.</remarks>
/// <param name="parameters">A dictionary containing parameter names and their corresponding values to be processed by the handler. Cannot be
/// null.</param>
/// <returns>A task that represents the asynchronous operation. The result contains an object returned by the intercepted method.</returns>
public delegate Task<object> InterceptAsyncMethodCallerHandler(Dictionary<string, object> parameters);

/// <summary>
/// Represents a method interception handler that can process or modify a method invocation before, after, or instead of
/// its original execution.
/// </summary>
/// <remarks>Use this delegate to implement custom logic such as logging, validation, or altering method behavior.
/// The handler can choose to invoke the original method via the provided delegate or bypass it entirely. Modifying the
/// parameters dictionary affects the arguments passed to the original method.</remarks>
/// <param name="methodName">The name of the method being intercepted. This value identifies the target method for the interception.</param>
/// <param name="method">A delegate representing the original method caller. Invoke this delegate to execute the original method
/// implementation.</param>
/// <param name="parameters">A dictionary containing the parameters to be passed to the method. Keys are parameter names; values are the
/// corresponding argument values.</param>
/// <returns>The result of the intercepted method invocation. The return value may be the result of the original method or a
/// custom value provided by the handler.</returns>
public delegate object InterceptMethodHandler(
    string methodName,
    InterceptMethodCallerHandler method,
    Dictionary<string, object> parameters
    );

/// <summary>
/// Represents a handler that intercepts an asynchronous method invocation, allowing custom logic to be executed before
/// or after the method call.
/// </summary>
/// <remarks>This delegate enables scenarios such as logging, validation, or modifying arguments and results
/// during asynchronous method execution. The handler can choose to invoke the original method via <paramref
/// name="methodAsync"/> or provide an alternative result.</remarks>
/// <param name="methodName">The name of the method being intercepted. This value identifies the target method for the interception.</param>
/// <param name="methodAsync">A delegate that invokes the original asynchronous method. Call this delegate to execute the underlying method logic.</param>
/// <param name="parameters">A dictionary containing the parameters to be passed to the method. Keys are parameter names; values are the
/// corresponding argument values.</param>
/// <returns>An object representing the result of the intercepted method call. The type and meaning of the result depend on the
/// intercepted method.</returns>
public delegate Task<object> InterceptAsyncMethodHandler(
    string methodName,
    InterceptAsyncMethodCallerHandler methodAsync,
    Dictionary<string, object> parameters
    );