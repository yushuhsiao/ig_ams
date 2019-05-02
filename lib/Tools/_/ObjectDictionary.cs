using _DebuggerStepThrough = System.Diagnostics.DebuggerStepThroughAttribute;
namespace System.Collections.Generic
{
    [_DebuggerStepThrough]
    public class ObjectDictionary : Dictionary<string, object>
    {
        public T GetValue<T>(string key)
        {
            object result;
            if (base.TryGetValue(key, out result))
                if (result is T)
                    return (T)result;
            return default(T);
        }
    }
}
