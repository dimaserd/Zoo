using Croco.Core.Contract;
using Croco.Core.Contract.Application;
using Croco.Core.Contract.Models;
using Croco.Core.Logic.Services;
using Ecc.Contract.Settings;
using Ecc.Logic.Resources;
using Ecc.Model.Contexts;
using Microsoft.Extensions.Logging;

namespace Ecc.Logic.Services.Base
{
    /// <summary>
    /// Базовый сервис контекста рассылок
    /// </summary>
    public class BaseEccService : BaseCrocoService<EccDbContext>
    {
        EccRolesSetting RolesSetting { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context"></param>
        /// <param name="application"></param>
        public BaseEccService(ICrocoAmbientContextAccessor context, ICrocoApplication application) : base(context, application)
        {
            RolesSetting = Application.SettingsFactory.GetSetting<EccRolesSetting>();
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context"></param>
        /// <param name="application"></param>
        /// <param name="logger"></param>
        public BaseEccService(ICrocoAmbientContextAccessor context, ICrocoApplication application, ILogger logger) : base(context, application, logger)
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