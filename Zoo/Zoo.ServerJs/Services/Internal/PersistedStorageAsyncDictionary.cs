using System.Collections.Generic;
using System.Threading.Tasks;
using Zoo.ServerJs.Abstractions;

namespace Zoo.ServerJs.Services.Internal
{
    internal class PersistedStorageAsyncDictionary<TValue>
    {
        private readonly IPersistedStorage _persistedStorage;
        private readonly string _key;

        public PersistedStorageAsyncDictionary(IPersistedStorage persistedStorage, string key, Dictionary<string, TValue> defaultValue)
        {
            _persistedStorage = persistedStorage;
            _key = key;
            SaveInternalValue(defaultValue);
        }

        public async Task<TValue> GetValueAsync(string key)
        {
            var dict = await GetInternalValueAsync();
            return dict[key];
        }

        public async Task SetValueAsync(string key, TValue value)
        {
            var dict = await GetInternalValueAsync();
            dict[key] = value;
            await SaveInternalValueAsync(dict);
        }

        public async Task RemoveAsync(string key)
        {
            var dict = await GetInternalValueAsync();

            dict.Remove(key);
            await SaveInternalValueAsync(dict);
        }

        public bool ContainsKey(string key)
        {
            var dict = GetInternalValue();

            return dict.ContainsKey(key);
        }

        public async Task<bool> ContainsKeyAsync(string key)
        {
            var dict = await GetInternalValueAsync();

            return dict.ContainsKey(key);
        }

        public Dictionary<string, TValue> GetInternalValue()
        {
            var result = _persistedStorage.GetValue<Dictionary<string, TValue>>(_key);

            if (!result.IsSucceeded)
            {
                return new Dictionary<string, TValue>();
            }

            return result.Value;
        }

        public async Task<Dictionary<string, TValue>> GetInternalValueAsync()
        {
            var result = await _persistedStorage.GetValueAsync<Dictionary<string, TValue>>(_key);

            if (!result.IsSucceeded)
            {
                return new Dictionary<string, TValue>();
            }

            return result.Value;
        }

        public void SaveInternalValue(Dictionary<string, TValue> value)
        {
            _persistedStorage.AddOrUpdateValue(_key, value);
        }

        public Task SaveInternalValueAsync(Dictionary<string, TValue> value)
        {
            return _persistedStorage.AddOrUpdateValueAsync(_key, value);
        }
    }
}