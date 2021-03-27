using System.Collections.Generic;
using System.Threading.Tasks;
using Tms.Logic.Models;

namespace Tms.Logic.Abstractions
{
    /// <summary>
    /// Абстракция файлового хранилища пользователей
    /// </summary>
    public interface IUsersStorage
    {
        /// <summary>
        /// Получить пользователя по идентификатору
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<UserFullNameEmailAndAvatarModel> GetUserById(string userId);

        /// <summary>
        /// Получить список пользователей (ключ является идентификатором)
        /// </summary>
        /// <returns></returns>
        Task<Dictionary<string, UserFullNameEmailAndAvatarModel>> GetUsersDictionary();
    }
}