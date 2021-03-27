using System.Threading.Tasks;

namespace Clt.Logic.Abstractions
{
    /// <summary>
    /// Менеджер для выхода из авторизованности
    /// </summary>
    public interface IApplicationAuthenticationManager
    {
        /// <summary>
        /// Разлогиниться
        /// </summary>
        /// <returns></returns>
        Task SignOutAsync();
    }
}