using Croco.Core.Contract.Models.Search;
using Ecc.Contract.Models.Users;
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

        /// <summary>
        /// Получить список пользователей
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<GetListResult<EccUserModel>> GetUsers(GetListSearchModel model);
    }
}