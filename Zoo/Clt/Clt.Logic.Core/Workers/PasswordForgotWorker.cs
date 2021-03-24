using Clt.Contract.Events;
using Clt.Contract.Models.Account;
using Clt.Contract.Models.Common;
using Clt.Contract.Settings;
using Croco.Core.Contract;
using Croco.Core.Contract.Application;
using Croco.Core.Contract.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Clt.Logic.Core.Workers
{
    public class PasswordForgotWorker<TUser, TDbContext> : BaseCltCoreWorker<TDbContext>
        where TUser : IdentityUser, new()
        where TDbContext : DbContext
    {
        public PasswordForgotWorker(ICrocoAmbientContextAccessor context, ICrocoApplication application) : base(context, application)
        {
        }

        
        #region Методы восстановления пароля

        public async Task<BaseApiResponse> UserForgotPasswordByEmailHandlerAsync(ForgotPasswordModel model, UserManager<TUser> userManager, Func<string, Task<ApplicationUserBaseModel>> userEmailFunc)
        {
            if (model == null)
            {
                return new BaseApiResponse(false, "Вы подали пустую модель");
            }

            if (IsAuthenticated)
            {
                return new BaseApiResponse(false, "Вы авторизованы в системе");
            }

            var user = await userEmailFunc(model.Email);

            if (user == null)
            {
                return new BaseApiResponse(false, $"Пользователь не найден по указанному электронному адресу {model.Email}");
            }

            return await UserForgotPasswordNotificateHandlerAsync(user.ToEntity<TUser>(), userManager);
        }

        public async Task<BaseApiResponse> UserForgotPasswordByPhoneHandlerAsync(ForgotPasswordModelByPhone model, UserManager<TUser> userManager, Func<string, Task<ApplicationUserBaseModel>> userByPhoneNumberFunc)
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

            var user = await userByPhoneNumberFunc(model.PhoneNumber);

            return await UserForgotPasswordNotificateHandlerAsync(user.ToEntity<TUser>(), userManager);
        }

        private async Task<BaseApiResponse> UserForgotPasswordNotificateHandlerAsync(TUser user, UserManager<TUser> userManager)
        {
            var accountSettings = GetSetting<AccountSettingsModel>();

            if (user == null || !user.EmailConfirmed && accountSettings.ShouldUsersConfirmEmail)
            {
                // Не показывать, что пользователь не существует или не подтвержден
                return new BaseApiResponse(false, "Пользователь не существует или его Email не подтверждён");
            }

            await userManager.UpdateSecurityStampAsync(user);

            // Отправка сообщения электронной почты с этой ссылкой
            var code = await userManager.GeneratePasswordResetTokenAsync(user);

            await PublishMessageAsync(new ClientStartedRestoringPasswordEvent
            {
                Code = HttpUtility.UrlEncode(code),
                UserId = user.Id
            });

            return new BaseApiResponse(true, "Ok");
        }

        public async Task<BaseApiResponse> ChangePasswordByToken(ChangePasswordByToken model, UserManager<TUser> userManager)
        {
            var user = await userManager.FindByIdAsync(model.UserId);

            if (user == null)
            {
                return new BaseApiResponse(false, "Пользователь не найден по указанному идентификатору");
            }

            var resp = await userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);

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