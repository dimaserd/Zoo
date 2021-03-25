using Clt.Model.Entities;
using Clt.Model.Entities.Default;

namespace Clt.Logic.Models
{
    /// <summary>
    /// Клиент объединенный с пользователем
    /// </summary>
    public class ClientJoinedWithApplicationUser
    {
        /// <summary>
        /// Клиент 
        /// </summary>
        public Client Client { get; set; }

        /// <summary>
        /// Пользователь
        /// </summary>
        public ApplicationUser User { get; set; }
    }
}