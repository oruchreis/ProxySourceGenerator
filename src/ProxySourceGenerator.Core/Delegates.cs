using System;
using System.Collections.Generic;
using System.Text;

namespace ProxySourceGenerator;

public delegate object InterceptPropertyGetterHandler(string propertyName, Func<object> getter);
public delegate void InterceptPropertySetterHandler(string propertyName, Action<object> setter, object value);

public delegate object InterceptMethodCallerHandler(Dictionary<string, object> parameters);
public delegate object InterceptMethodHandler(
    string methodName,
    InterceptMethodCallerHandler method,
    Dictionary<string, object> parameters
    );
