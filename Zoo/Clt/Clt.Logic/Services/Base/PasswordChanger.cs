using Clt.Contract.Models.Account;
using Clt.Logic.Extensions;
using Clt.Logic.Services.Users;
using Clt.Model.Entities.Default;
using Croco.Core.Contract;
using Croco.Core.Contract.Application;
using Croco.Core.Contract.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Clt.Logic.Services.Base
{
    /// <summary>
    /// Сервис для изменеия пароля
    /// </summary>
    public class PasswordChanger : BaseCltService
    {
        UserManager<ApplicationUser> UserManager { get; }
        SignInManager<ApplicationUser> SignInManager { get; }
        UserSearcher UserSearcher { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context"></param>
        /// <param name="application"></param>
        /// <param name="userManager"></param>
        /// <param name="signInManager"></param>
        /// <param name="userSearcher"></param>
        public PasswordChanger(ICrocoAmbientContextAccessor context, ICrocoApplication application,
            UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            UserSearcher userSearcher) : base(context, application)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            UserSearcher = userSearcher;
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

        /// <summary>
        /// Изменить пароль администратором
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<BaseApiResponse> ChangePasswordByAdminAsync(ResetPasswordByAdminModel model)
        {
            var user = await UserManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                return new BaseApiResponse(false, "Пользователь не найден");
            }

            var userDto = await UserSearcher.GetUserByIdAsync(user.Id);

            var result = UserRightsExtensions.HasRightToEditUser(userDto, User, RolesSetting);

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
        private async Task<BaseApiResponse> ChangePasswordBaseAsync(ApplicationUser user, string newPassword)
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