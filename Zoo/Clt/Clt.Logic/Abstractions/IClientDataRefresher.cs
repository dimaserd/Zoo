using Clt.Model.Entities;
using Clt.Model.Entities.Default;
using System.Threading.Tasks;

namespace Clt.Logic.Abstractions
{
    /// <summary>
    /// Обновитель клиентских данных хранящихся в авторизации
    /// </summary>
    public interface IClientDataRefresher
    {
        /// <summary>
        /// Обновить данные пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        Task UpdateUserDataAsync(ApplicationUser user, Client client);
    }
}