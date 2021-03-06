﻿using System;
using System.Threading.Tasks;
using Clt.Contract.Models.Account;
using Clt.Logic.Services.Users;
using Clt.Logic.Models.Account;
using Clt.Logic.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Clt.Contract.Settings;
using Croco.Core.Contract.Models;
using Croco.Core.Contract;
using Croco.Core.Contract.Application;
using Clt.Model.Entities;
using Clt.Model.Entities.Default;
using Microsoft.Extensions.Logging;

namespace Clt.Logic.Services.Account
{
    /// <summary>
    /// Сервис для работы с методами логинирования
    /// </summary>
    public class AccountLoginService : BaseCltService
    {
        SignInManager<ApplicationUser> SignInManager { get; }
        UserSearcher UserSearcher { get; }
        PasswordHashValidator PasswordHashValidator { get; }
        IApplicationAuthenticationManager AuthenticationManager { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="ambientContext"></param>
        /// <param name="application"></param>
        /// <param name="signInManager"></param>
        /// <param name="userSearcher"></param>
        /// <param name="passwordHashValidator"></param>
        /// <param name="authenticationManager"></param>
        /// <param name="logger"></param>
        public AccountLoginService(ICrocoAmbientContextAccessor ambientContext,
            ICrocoApplication application,
            SignInManager<ApplicationUser> signInManager,
            UserSearcher userSearcher,
            PasswordHashValidator passwordHashValidator,
            IApplicationAuthenticationManager authenticationManager,
            ILogger<AccountLoginService> logger) : base(ambientContext, application, logger)
        {
            SignInManager = signInManager;
            UserSearcher = userSearcher;
            PasswordHashValidator = passwordHashValidator;
            AuthenticationManager = authenticationManager;
        }


        /// <summary>
        /// Авторизоваться по Email
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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

            var user = await SignInManager.UserManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                Logger.LogTrace("user is null");
                return UnSuccessfulLoginAttempt();
            }

            if(user.Email == RootSettings.RootEmail)
            {
                Logger.LogTrace("AccountLoginWorker.LoginAsync.RootCase");

                if (model.Password != RootSettings.RootPassword)
                {
                    return UnSuccessfulLoginAttempt();
                }

                await SignInManager.SignInAsync(user, model.RememberMe);
                return SuccessfulLoginResult();
            }

            var checkClientResult = await CheckClient(model.Email);

            if (!checkClientResult.IsSucceeded)
            {
                return checkClientResult;
            }

            var accountSettings = GetSetting<AccountSettingsModel>();

            //если логинирование не разрешено для пользователей которые не подтвердили Email и у пользователя Email не потверждён
            if (!user.EmailConfirmed && !accountSettings.IsLoginEnabledForUsersWhoDidNotConfirmEmail)
            {
                return new BaseApiResponse<LoginResultModel>(false, "Ваш Email не подтверждён", new LoginResultModel { Result = LoginResult.EmailNotConfirmed });
            }

            try
            {
                //проверяю пароль
                var passCheckResult = await PasswordHashValidator
                    .CheckUserNameAndPasswordAsync(user.Id, user.UserName, model.Password);

                Logger.LogTrace("AccountLoginWorker.LoginAsync.PassCheckResult", passCheckResult);

                //если пароль не подходит выдаю ответ
                if (!passCheckResult.IsSucceeded)
                {
                    return UnSuccessfulLoginAttempt();
                }

                await SignInManager.SignInAsync(user, model.RememberMe);

                return SuccessfulLoginResult();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "AccountLoginWorker.LoginAsync.OnException");

                return new BaseApiResponse<LoginResultModel>(false, ex.Message);
            }
        }


        private static BaseApiResponse<LoginResultModel> UnSuccessfulLoginAttempt()
        {
            return new BaseApiResponse<LoginResultModel>(false, "Неудачная попытка входа", new LoginResultModel { Result = LoginResult.UnSuccessfulAttempt, TokenId = null });
        }

        private static BaseApiResponse<LoginResultModel> SuccessfulLoginResult()
        {
            return new BaseApiResponse<LoginResultModel>(true, "Вы успешно авторизованы", new LoginResultModel { Result = LoginResult.SuccessfulLogin });
        }


        /// <summary>
        /// Авторизоваться по номеру телефона
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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

            return await LoginAsync(GetLoginModel(model, user.Email));
        }

        /// <summary>
        /// Разлогинивание в системе
        /// </summary>
        /// <returns></returns>
        public async Task<BaseApiResponse> LogOut()
        {
            if(!IsAuthenticated)
            {
                return new BaseApiResponse(false, "Вы и так не авторизованы");
            }

            await AuthenticationManager.SignOutAsync();

            return new BaseApiResponse(true, "Вы успешно разлогинены в системе");
        }

        private static LoginModel GetLoginModel(LoginByPhoneNumberModel model, string email)
        {
            return new LoginModel
            {
                Email = email,
                Password = model.Password,
                RememberMe = model.RememberMe
            };
        }

        private async Task<BaseApiResponse<LoginResultModel>> CheckClient(string email)
        {
            var client = await Query<Client>()
                .FirstOrDefaultAsync(x => x.Email == email);

            if (client == null)
            {
                Logger.LogTrace("client is null");
                return UnSuccessfulLoginAttempt();
            }

            if (client.DeActivated)
            {
                return new BaseApiResponse<LoginResultModel>(false, "Ваша учетная запись деактивирована", new LoginResultModel { Result = LoginResult.UserDeactivated });
            }

            return new BaseApiResponse<LoginResultModel>(true, "Ok");
        }
    }
}