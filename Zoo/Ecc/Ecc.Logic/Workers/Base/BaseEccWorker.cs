using Croco.Core.Contract;
using Croco.Core.Contract.Application;
using Croco.Core.Contract.Models;
using Croco.Core.Logic.Services;
using Ecc.Contract.Settings;
using Ecc.Logic.Resources;
using Ecc.Model.Contexts;

namespace Ecc.Logic.Workers.Base
{
    /// <summary>
    /// Базовый сервис контекста рассылок
    /// </summary>
    public class BaseEccWorker : BaseCrocoService<EccDbContext>
    {
        EccRolesSetting RolesSetting { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context"></param>
        /// <param name="application"></param>
        public BaseEccWorker(ICrocoAmbientContextAccessor context, ICrocoApplication application) : base(context, application)
        {
            RolesSetting = Application.SettingsFactory.GetSetting<EccRolesSetting>();
        }

        /// <summary>
        /// Является ли пользователь админом
        /// </summary>
        /// <returns></returns>
        public bool IsUserAdmin()
        {
            return User.IsInRole(RolesSetting.AdminRoleName);
        }

        /// <summary>
        /// Валидировать модель и проверить пользователя на админа
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseApiResponse ValidateModelAndUserIsAdmin(object model)
        {
            if (!IsUserAdmin())
            {
                return new BaseApiResponse(false, ValidationMessages.YouAreNotAnAdministrator);
            }

            return ValidateModel(model);
        }
    }
}