using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Zoo.GenericUserInterface.Extensions
{
    internal static class DictionaryExtensions
    {
        internal static async Task<TValue> GetOrAddAsync<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<Task<TValue>> getValue)
        {
            if (dictionary.ContainsKey(key))
            {
                return dictionary[key];
            }

            var value = await getValue();
            dictionary[key] = value;
            return value;
        }
    }
}