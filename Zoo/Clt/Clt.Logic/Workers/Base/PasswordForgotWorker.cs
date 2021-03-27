using Clt.Contract.Events;
using Clt.Contract.Models.Account;
using Clt.Contract.Settings;
using Clt.Logic.Workers;
using Clt.Model.Entities.Default;
using Croco.Core.Contract;
using Croco.Core.Contract.Application;
using Croco.Core.Contract.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Clt.Logic.Core.Workers
{
    /// <summary>
    /// Предоставляет методы для работы с забывшими пароль пользователями
    /// </summary>
    public class PasswordForgotWorker : BaseCltWorker
    {
        UserManager<ApplicationUser> UserManager { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context"></param>
        /// <param name="application"></param>
        /// <param name="userManager"></param>
        public PasswordForgotWorker(ICrocoAmbientContextAccessor context, 
            ICrocoApplication application,
            UserManager<ApplicationUser> userManager
            ) : base(context, application)
        {
            UserManager = userManager;
        }
        
        #region Методы восстановления пароля

        /// <summary>
        /// Востановить пароль через адрес электронной почты
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<BaseApiResponse> UserForgotPassword(ForgotPasswordModel model)
        {
            if (model == null)
            {
                return new BaseApiResponse(false, "Вы подали пустую модель");
            }

            if (IsAuthenticated)
            {
                return new BaseApiResponse(false, "Вы авторизованы в системе");
            }

            var user = await Query<ApplicationUser>()
                .FirstOrDefaultAsync(x => x.Email == model.Email);

            if (user == null)
            {
                return new BaseApiResponse(false, $"Пользователь не найден по указанному электронному адресу {model.Email}");
            }

            return await UserForgotPasswordNotificateHandlerAsync(user);
        }

        /// <summary>
        /// Востановить пароль через номер телефона
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<BaseApiResponse> UserForgotPasswordByPhoneHandlerAsync(ForgotPasswordModelByPhone model)
        {
            var validation = ValidateModel(model);

            if (!validation.IsSucceeded)
            {
                return validation;
            }

            if (IsAuthenticated)
            {
                return new BaseApiResponse(false, "Вы авторизованы в системе");
            }

            var user = await Query<ApplicationUser>()
                .FirstOrDefaultAsync(x => x.PhoneNumber == model.PhoneNumber);

            if (user == null)
            {
                return new BaseApiResponse(false, $"Пользователь не найден по указанному номеру телефона {model.PhoneNumber}");
            }

            return await UserForgotPasswordNotificateHandlerAsync(user);
        }

        private async Task<BaseApiResponse> UserForgotPasswordNotificateHandlerAsync(ApplicationUser user)
        {
            var accountSettings = GetSetting<AccountSettingsModel>();

            if (user == null || !user.EmailConfirmed && accountSettings.ShouldUsersConfirmEmail)
            {
                // Не показывать, что пользователь не существует или не подтвержден
                return new BaseApiResponse(false, "Пользователь не существует или его Email не подтверждён");
            }

            await UserManager.UpdateSecurityStampAsync(user);

            // Отправка сообщения электронной почты с этой ссылкой
            var code = await UserManager.GeneratePasswordResetTokenAsync(user);

            await PublishMessageAsync(new ClientStartedRestoringPasswordEvent
            {
                Code = HttpUtility.UrlEncode(code),
                UserId = user.Id
            });

            return new BaseApiResponse(true, "Ok");
        }

        /// <summary>
        /// Изменить пароль по токену
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<BaseApiResponse> ChangePasswordByToken(ChangePasswordByToken model)
        {
            var user = await UserManager.FindByIdAsync(model.UserId);

            if (user == null)
            {
                return new BaseApiResponse(false, "Пользователь не найден по указанному идентификатору");
            }

            var resp = await UserManager.ResetPasswordAsync(user, model.Token, model.NewPassword);

            if (!resp.Succeeded)
            {
                return new BaseApiResponse(false, resp.Errors.First().Description);
            }

            await PublishMessageAsync(new ClientChangedPassword
            {
                ClientId = user.Id
            });

            return new BaseApiResponse(true, "Ваш пароль был изменён");
        }
        #endregion
    }
}