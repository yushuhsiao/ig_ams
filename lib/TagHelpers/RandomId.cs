using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace InnateGlory
{
    partial class TagHelperExtensions
    {
        public static IMvcBuilder AddRandomId(this IMvcBuilder mvc)
        {
            mvc.Services.TryAddTransient<RandomId>();
            return mvc;
        }
    }

    public class RandomId
    {
        private Dictionary<object, string> _values = new Dictionary<object, string>();

        public string this[object key, int length = 20]
        {
            get
            {
                object _key = key ?? _values;
                lock (_values)
                {
                    if (_values.TryGetValue(key, out string result))
                        return result;
                    if (length < 5)
                        length = 5;
                    for (int i = 0; i < 100; i++)
                    {
                        result = $"{RandomValue.GetRandomString(1, RandomValue.LowerLetter)}{RandomValue.GetRandomString(length - 1)}";
                        if (!_values.ContainsValue(result))
                            return _values[_key] = result;
                    }
                    return _key.ToString();
                }
            }
        }
    }
}