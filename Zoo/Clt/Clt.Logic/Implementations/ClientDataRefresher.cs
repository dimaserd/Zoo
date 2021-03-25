using System.Threading.Tasks;
using Clt.Logic.Abstractions;
using Clt.Model.Entities;
using Clt.Model.Entities.Default;
using Microsoft.AspNetCore.Identity;

namespace Clt.Logic.Implementations
{
    /// <summary>
    /// Обновитель клиентских данных хранящихся в авторизации
    /// </summary>
    public class ClientDataRefresher : IClientDataRefresher
    {
        SignInManager<ApplicationUser> SignInManager { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="signInManager"></param>
        public ClientDataRefresher(SignInManager<ApplicationUser> signInManager)
        {
            SignInManager = signInManager;
        }

        /// <summary>
        /// Обновить данные пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        public Task UpdateUserDataAsync(ApplicationUser user, Client client)
        {
            return SignInManager.SignInAsync(user, true);
        }
    }
}