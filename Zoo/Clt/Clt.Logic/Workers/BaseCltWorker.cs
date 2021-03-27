using Clt.Contract.Settings;
using Clt.Logic.Resources;
using Clt.Model;
using Croco.Core.Contract;
using Croco.Core.Contract.Application;
using Croco.Core.Contract.Models;
using Croco.Core.Logic.Workers;

namespace Clt.Logic.Workers
{
    /// <summary>
    /// Базовый сервис для клиентского контекста
    /// </summary>
    public class BaseCltWorker : BaseCrocoWorker<CltDbContext>
    {
        /// <summary>
        /// Настройки клиенстких ролей
        /// </summary>
        protected CltRolesSetting RolesSetting { get; }

        /// <summary>
        /// Настройки прав приложения
        /// </summary>
        protected RootSettings RootSettings { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context"></param>
        /// <param name="application"></param>
        public BaseCltWorker(ICrocoAmbientContextAccessor context, ICrocoApplication application) : base(context, application)
        {
            var settingsFactory = Application.SettingsFactory;
            RolesSetting = settingsFactory.GetSetting<CltRolesSetting>();
            RootSettings = settingsFactory.GetSetting<RootSettings>();
        }

        /// <summary>
        /// Является ли пользователь рутом
        /// </summary>
        /// <returns></returns>
        public bool IsUserRoot()
        {
            return User.IsInRole(RolesSetting.RootRoleName);
        }

        /// <summary>
        /// Является ли пользователь администратором
        /// </summary>
        /// <returns></returns>
        public bool IsUserAdmin()
        {
            return User.IsInRole(RolesSetting.AdminRoleName);
        }

        /// <summary>
        /// Валидировать модель и убедиться что пользователь является админом
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseApiResponse ValidateModelAndUserIsAdmin(object model)
        {
            var right = IsUserAdmin();

            if (!right)
            {
                return new BaseApiResponse(false, ValidationMessages.YouAreNotAnAdministrator);
            }

            return ValidateModel(model);
        }
    }
}