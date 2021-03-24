using Clt.Contract.Models.Account;
using Clt.Contract.Models.Common;
using Croco.Core.Contract;
using Croco.Core.Contract.Application;
using Croco.Core.Contract.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Clt.Logic.Core.Workers
{
    public class PasswordChanger<TUser, TDbContext> : BaseCltCoreWorker<TDbContext> 
        where TUser : IdentityUser, new()
        where TDbContext : DbContext
    {
        UserManager<TUser> UserManager { get; }
        SignInManager<TUser> SignInManager { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context"></param>
        /// <param name="application"></param>
        /// <param name="userManager"></param>
        /// <param name="signInManager"></param>
        public PasswordChanger(ICrocoAmbientContextAccessor context, ICrocoApplication application,
            UserManager<TUser> userManager, SignInManager<TUser> signInManager) : base(context, application)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        /// <summary>
        /// Изменить пароль
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<BaseApiResponse> ChangePasswordAsync(ChangeUserPasswordModel model)
        {
            if (!IsAuthenticated)
            {
                return new BaseApiResponse(false, "Вы не авторизованы");
            }

            var validation = ValidateModel(model);

            if (!validation.IsSucceeded)
            {
                return validation;
            }

            if (model.NewPassword == model.OldPassword)
            {
                return new BaseApiResponse(false, "Новый и старый пароль совпадют");
            }

            var user = await UserManager.FindByIdAsync(UserId);

            if (user == null)
            {
                return new BaseApiResponse(false, "Пользователь не найден по указанному идентификатору");
            }

            var result = await UserManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                return new BaseApiResponse(false, "Неправильно указан старый пароль");
            }

            if (user != null)
            {
                await SignInManager.SignInAsync(user, true);
            }

            return new BaseApiResponse(true, "Ваш пароль изменен");
        }

        public async Task<BaseApiResponse> ChangePasswordByAdminAsync(ResetPasswordByAdminModel model, Func<string, Task<ApplicationUserBaseModel>> getUserByIdFunc)
        {
            var user = await UserManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                return new BaseApiResponse(false, "Пользователь не найден");
            }

            var userDto = await getUserByIdFunc(user.Id);

            var result = UserRightsWorker.HasRightToEditUser(userDto, User, RolesSetting);

            if (!result.IsSucceeded)
            {
                return result;
            }

            return await ChangePasswordBaseAsync(user, model.Password);
        }

        /// <summary>
        /// Данный метод не может быть вынесен в API (Базовый метод)
        /// </summary>
        /// <param name="user"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public async Task<BaseApiResponse> ChangePasswordBaseAsync(TUser user, string newPassword)
        {
            if (user == null)
            {
                return new BaseApiResponse(false, "Пользователь не найден");
            }

            var code = await UserManager.GeneratePasswordResetTokenAsync(user);

            var resetResult = await UserManager.ResetPasswordAsync(user, code, newPassword);

            if (!resetResult.Succeeded)
            {
                return new BaseApiResponse(resetResult.Succeeded, resetResult.Errors.First().Description);
            }

            return new BaseApiResponse(true, $"Вы изменили пароль для пользователя {user.Email}");
        }
    }
}