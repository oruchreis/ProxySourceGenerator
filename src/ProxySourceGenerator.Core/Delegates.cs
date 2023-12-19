using System;
using System.Collections.Generic;

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
/// Delegate for calling a method.
/// </summary>
/// <param name="parameters"></param>
/// <returns></returns>
public delegate object InterceptMethodCallerHandler(Dictionary<string, object> parameters);
/// <summary>
/// Delegate of intercepting a method.
/// </summary>
/// <param name="methodName"></param>
/// <param name="method"></param>
/// <param name="parameters"></param>
/// <returns></returns>
public delegate object InterceptMethodHandler(
    string methodName,
    InterceptMethodCallerHandler method,
    Dictionary<string, object> parameters
    );
