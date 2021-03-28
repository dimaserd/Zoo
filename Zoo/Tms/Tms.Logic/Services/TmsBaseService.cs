using Croco.Core.Contract;
using Croco.Core.Contract.Application;
using Croco.Core.Logic.Services;
using Tms.Model;

namespace Tms.Logic.Services
{
    /// <summary>
    /// Базовый класс для менеджера заданий
    /// </summary>
    public class TmsBaseService : BaseCrocoService<TmsDbContext>
    {
        PrincipalCheker PrincipalCheker { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="contextAccessor"></param>
        /// <param name="application"></param>
        /// <param name="principalCheker"></param>
        public TmsBaseService(ICrocoAmbientContextAccessor contextAccessor, 
            ICrocoApplication application,
            PrincipalCheker principalCheker) : base(contextAccessor, application)
        {
            PrincipalCheker = principalCheker;
        }

        /// <summary>
        /// Проверка на администратора
        /// </summary>
        /// <returns></returns>
        public bool IsUserAdmin()
        {
            return PrincipalCheker.IsAdmin(User);
        }
    }
}