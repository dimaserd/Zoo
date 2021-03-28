using Ecc.Contract.Models;
using System.Threading.Tasks;

namespace Ecc.Contract.Abstractions
{
    /// <summary>
    /// Хранилище пользовательских данных от которого будет синхронизироваться контекст рассылок
    /// </summary>
    public interface IUserMasterStorage
    {
        /// <summary>
        /// Получить пользователя по идентификатору
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<EccUserModel> GetUserById(string userId);
    }
}