using System.Threading.Tasks;
using Clt.Logic.Abstractions;
using Clt.Model.Entities.Default;
using Microsoft.AspNetCore.Identity;

namespace Clt.Logic.Implementations
{
    /// <summary>
    /// Менеджер авторизациии для приложения
    /// </summary>
    public class ApplicationAuthenticationManager : IApplicationAuthenticationManager
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="signInManager"></param>
        public ApplicationAuthenticationManager(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        /// <summary>
        /// Выйти
        /// </summary>
        /// <returns></returns>
        public Task SignOutAsync()
        {
            return _signInManager.SignOutAsync();
        }
    }
}