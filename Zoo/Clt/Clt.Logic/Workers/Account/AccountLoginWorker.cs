using System;
using System.Threading.Tasks;
using Clt.Contract.Models.Account;
using Clt.Logic.Workers.Users;
using Clt.Logic.Models.Account;
using Clt.Logic.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Clt.Contract.Settings;
using Croco.Core.Contract.Models;
using Croco.Core.Contract;
using Croco.Core.Contract.Application;
using Microsoft.Extensions.Logging;
using Clt.Logic.Core.Workers;
using Clt.Model.Entities;
using Clt.Logic.Settings;
using Clt.Model.Entities.Default;
using Clt.Model;

namespace Clt.Logic.Workers.Account
{
    public class AccountLoginWorker : BaseCltWorker
    {
        SignInManager<ApplicationUser> SignInManager { get; }
        UserSearcher UserSearcher { get; }
        PasswordHashValidator<ApplicationUser, CltDbContext> PasswordHashValidator { get; }

        public AccountLoginWorker(ICrocoAmbientContextAccessor ambientContext, 
            ICrocoApplication application,
            SignInManager<ApplicationUser> signInManager,
            UserSearcher userSearcher,
            PasswordHashValidator<ApplicationUser, CltDbContext> passwordHashValidator) : base(ambientContext, application)
        {
            SignInManager = signInManager;
            UserSearcher = userSearcher;
            PasswordHashValidator = passwordHashValidator;
        }

        public async Task<BaseApiResponse<LoginResultModel>> LoginAsync(LoginModel model)
        {
            var validation = ValidateModel(model);

            if (!validation.IsSucceeded)
            {
                return new BaseApiResponse<LoginResultModel>(validation);
            }

            if (IsAuthenticated)
            {
                return new BaseApiResponse<LoginResultModel>(false, "Вы уже авторизованы в системе", new LoginResultModel { Result = LoginResult.AlreadyAuthenticated });
            }

            model.RememberMe = true;

            var result = false;

            var user = await SignInManager.UserManager.FindByEmailAsync(model.Email);

            var client = await Query<Client>()
                .FirstOrDefaultAsync(x => x.Email == model.Email);

            if (user == null || client == null)
            {
                if(user != null && client == null)
                {
                    Logger.LogError(new Exception($"There is user without client {user.Id}"), "AccountLoginWorker.LoginAsync.OnException");
                }

                return new BaseApiResponse<LoginResultModel>(false, "Неудачная попытка входа", new LoginResultModel { Result = LoginResult.UnSuccessfulAttempt });
            }

            if (client.DeActivated)
            {
                return new BaseApiResponse<LoginResultModel>(false, "Ваша учетная запись деактивирована", new LoginResultModel { Result = LoginResult.UserDeactivated });
            }

            var accountSettings = GetSetting<AccountSettingsModel>();

            //если логинирование не разрешено для пользователей которые не подтвердили Email и у пользователя Email не потверждён
            if (user.Email != RightsSettings.RootEmail && !user.EmailConfirmed && !accountSettings.IsLoginEnabledForUsersWhoDidNotConfirmEmail)
            {
                return new BaseApiResponse<LoginResultModel>(false, "Ваш Email не подтверждён", new LoginResultModel { Result = LoginResult.EmailNotConfirmed });
            }

            try
            {
                //проверяю пароль
                var passCheckResult = await PasswordHashValidator
                    .CheckUserNameAndPasswordAsync(user.Id, user.UserName, model.Password);

                //если пароль не подходит выдаю ответ
                if (!passCheckResult.IsSucceeded)
                {
                    return new BaseApiResponse<LoginResultModel>(false, "Неудачная попытка входа", new LoginResultModel { Result = LoginResult.UnSuccessfulAttempt, TokenId = null });
                }
                
                if (user.Email == RightsSettings.RootEmail) //root входит без подтверждений
                {
                    await SignInManager.SignInAsync(user, model.RememberMe);

                    return new BaseApiResponse<LoginResultModel>(true, "Вы успешно авторизованы", new LoginResultModel { Result = LoginResult.SuccessfulLogin });
                }

                if (accountSettings.ConfirmLogin == AccountSettingsModel.ConfirmLoginType.None) //не логинить пользователя если нужно подтвержать логин
                {
                    await SignInManager.SignInAsync(user, model.RememberMe);

                    result = true;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "AccountLoginWorker.LoginAsync.OnException");

                return new BaseApiResponse<LoginResultModel>(false, ex.Message);
            }
            
            if (result)
            {   
                return new BaseApiResponse<LoginResultModel>(true, "Авторизация прошла успешно", new LoginResultModel { Result = LoginResult.SuccessfulLogin, TokenId = null });
            }

            return new BaseApiResponse<LoginResultModel>(false, "Неудачная попытка входа", new LoginResultModel { Result = LoginResult.UnSuccessfulAttempt, TokenId = null });
        }

        public async Task<BaseApiResponse<LoginResultModel>> LoginByPhoneNumberAsync(LoginByPhoneNumberModel model)
        {
            var validation = ValidateModel(model);

            if (!validation.IsSucceeded)
            {
                return new BaseApiResponse<LoginResultModel>(validation);
            }

            var user = await UserSearcher.GetUserByPhoneNumberAsync(model.PhoneNumber);

            if (user == null)
            {
                return new BaseApiResponse<LoginResultModel>(false, "Пользователь не найден по указанному номеру телефона");
            }

            return await LoginAsync(LoginModel.GetModel(model, user.Email));
        }

        /// <summary>
        /// Разлогинивание в системе
        /// </summary>
        /// <param name="user"></param>
        /// <param name="authenticationManager"></param>
        /// <returns></returns>
        public async Task<BaseApiResponse> LogOut(IApplicationAuthenticationManager authenticationManager)
        {
            if(!IsAuthenticated)
            {
                return new BaseApiResponse(false, "Вы и так не авторизованы");
            }

            await authenticationManager.SignOutAsync();

            return new BaseApiResponse(true, "Вы успешно разлогинены в системе");
        }
    }
}