using System.Threading.Tasks;
using Croco.Core.Contract.Models;

namespace Zoo.ServerJs.Abstractions
{
    /// <summary>
    /// Хранилище с постоянными данными
    /// </summary>
    public interface IPersistedStorage
    {
        /// <summary>
        /// Добавление значения в хранилище
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void AddOrUpdateValue<T>(string key, T value);

        /// <summary>
        /// Добавление значения в хранилище
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task AddOrUpdateValueAsync<T>(string key, T value);

        /// <summary>
        /// Получение значения из хранилища по ключу
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        CrocoSafeValue<T> GetValue<T>(string key);

        /// <summary>
        /// Получение значения из хранилища по ключу
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<CrocoSafeValue<T>> GetValueAsync<T>(string key);

        /// <summary>
        /// Удаление значения из кеша по ключу
        /// </summary>
        /// <param name="key"></param>
        void Remove(string key);

        /// <summary>
        /// Удаление значения из кеша по ключу
        /// </summary>
        /// <param name="key"></param>
        Task RemoveAsync(string key);
    }
}