using System;
using System.IO;
using System.Threading.Tasks;
using Croco.Core.Contract.Models;
using Newtonsoft.Json;
using Zoo.ServerJs.Abstractions;

namespace Zoo.ServerJs.Services
{
    /// <summary>
    /// Персистентное хранилище в файловой системе
    /// </summary>
    public class FilePersistedStorage : IPersistedStorage
    {
        string BaseDirectory { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public FilePersistedStorage() : this($"{AppContext.BaseDirectory}\\Js-OpenApi-Persisted-Storage")
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="baseDirectory"></param>
        public FilePersistedStorage(string baseDirectory)
        {
            BaseDirectory = baseDirectory;
            Directory.CreateDirectory(BaseDirectory);
        }

        /// <inheritdoc />
        public Task AddOrUpdateValueAsync<T>(string key, T value)
        {
            var json = JsonConvert.SerializeObject(value);
            return File.WriteAllTextAsync(GetFilePathByKey(key), json);
        }

        /// <inheritdoc />
        public async Task<CrocoSafeValue<T>> GetValueAsync<T>(string key)
        {
            var filePath = GetFilePathByKey(key);
            if (!File.Exists(filePath))
            {
                return new CrocoSafeValue<T>(false, default);
            }

            var json = await File.ReadAllTextAsync(filePath);

            var data = JsonConvert.DeserializeObject<T>(json);

            return new CrocoSafeValue<T>(true, data);
        }

        /// <inheritdoc />
        public Task RemoveAsync(string key)
        {
            Remove(key);
            return Task.CompletedTask;
        }

        private string GetFilePathByKey(string key)
        {
            return $"{BaseDirectory}\\{key}.json";
        }

        /// <inheritdoc />
        public void AddOrUpdateValue<T>(string key, T value)
        {
            var json = JsonConvert.SerializeObject(value);
            File.WriteAllText(GetFilePathByKey(key), json);
        }

        /// <inheritdoc />
        public CrocoSafeValue<T> GetValue<T>(string key)
        {
            var filePath = GetFilePathByKey(key);
            if (!File.Exists(filePath))
            {
                return new CrocoSafeValue<T>(false, default);
            }

            var json = File.ReadAllText(filePath);

            var data = JsonConvert.DeserializeObject<T>(json);

            return new CrocoSafeValue<T>(true, data);
        }

        /// <inheritdoc />
        public void Remove(string key)
        {
            var filePath = GetFilePathByKey(key);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
