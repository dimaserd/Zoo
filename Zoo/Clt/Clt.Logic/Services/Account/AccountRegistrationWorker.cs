using System;
using System.Linq;
using System.Threading.Tasks;
using Clt.Contract.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Clt.Contract.Models.Account;
using Clt.Contract.Models.Users;
using Clt.Contract.Settings;
using Croco.Core.Contract.Models;
using Croco.Core.Contract;
using Croco.Core.Contract.Application;
using Clt.Model.Entities.Default;
using Clt.Model.Entities;
using Clt.Contract.Models.Common;
using Clt.Logic.Services.Users;
using Microsoft.Extensions.Logging;

namespace Clt.Logic.Services.Account
{
    /// <summary>
    /// Методы для регистрации
    /// </summary>
    public class AccountRegistrationWorker : BaseCltService
    {
        UserManager<ApplicationUser> UserManager { get; }
        SignInManager<ApplicationUser> SignInManager { get; }
        UserSearcher UserSearcher { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="ambientContext"></param>
        /// <param name="application"></param>
        /// <param name="userManager"></param>
        /// <param name="signInManager"></param>
        /// <param name="userSearcher"></param>
        /// <param name="logger"></param>
        public AccountRegistrationWorker(ICrocoAmbientContextAccessor ambientContext,
            ICrocoApplication application,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            UserSearcher userSearcher,
            ILogger<AccountRegistrationWorker> logger) : base(ambientContext, application, logger)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            UserSearcher = userSearcher;
        }

        #region Методы регистрации

        /// <summary>
        /// Зарегистрировать пользователя и авторизоаться
        /// </summary>
        /// <param name="model"></param>
        /// <param name="createRandomPassword"></param>
        /// <returns></returns>
        public async Task<BaseApiResponse<RegisteredUser>> RegisterAndSignInAsync(RegisterModel model, bool createRandomPassword)
        {
            var result = await RegisterInnerAsync(model, createRandomPassword);

            if (!result.IsSucceeded)
            {
                return new BaseApiResponse<RegisteredUser>(result);
            }

            try
            {
                var userModel = result.ResponseObject;

                var user = await UserManager.Users.FirstOrDefaultAsync(x => x.Id == userModel.Id);
                
                //авторизация пользователя в системе. через распаковку во внутренную модель
                await SignInManager.SignInAsync(user, false);

                return new BaseApiResponse<RegisteredUser>(true, "Регистрация и Авторизация прошла успешно", new RegisteredUser
                {
                    Id = user.Id,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber
                });
            }
            catch (Exception ex)
            {
                return new BaseApiResponse<RegisteredUser>(ex);
            }
        }

        /// <summary>
        /// Зарегистрироваться
        /// </summary>
        /// <param name="model"></param>
        /// <param name="createRandomPass"></param>
        /// <returns></returns>
        public async Task<BaseApiResponse<RegisteredUser>> RegisterAsync(RegisterModel model, bool createRandomPass)
        {
            var result = await RegisterInnerAsync(model, createRandomPass);

            if(!result.IsSucceeded)
            {
                return new BaseApiResponse<RegisteredUser>(result);
            }

            return new BaseApiResponse<RegisteredUser>(result.IsSucceeded, result.Message, new RegisteredUser
            {
                Id = result.ResponseObject.Id,
                Email = result.ResponseObject.Email,
                PhoneNumber = result.ResponseObject.PhoneNumber
            });
        }

        private async Task<BaseApiResponse<ApplicationUserBaseModel>> RegisterInnerAsync(RegisterModel model, bool createRandomPassword)
        {
            var accountSettings = GetSetting<AccountSettingsModel>();

            if (!accountSettings.RegistrationEnabled)
            {
                return new BaseApiResponse<ApplicationUserBaseModel>(false, "В данном приложении запрещена регистрация");
            }

            if (IsAuthenticated)
            {
                return new BaseApiResponse<ApplicationUserBaseModel>(false, "Вы не можете регистрироваться, так как вы авторизованы в системе");
            }

            return await RegisterUserWithNoChecksAsync(model, createRandomPassword);
        }

        /// <summary>
        /// Зарегистрировать пользователя без всяких проверок
        /// </summary>
        /// <param name="model"></param>
        /// <param name="createRandomPassword"></param>
        /// <returns></returns>
        public async Task<BaseApiResponse<ApplicationUserBaseModel>> RegisterUserWithNoChecksAsync(RegisterModel model, bool createRandomPassword)
        {
            if (createRandomPassword)
            {
                model.Password = "testpass";
            }

            var result = await RegisterHelpMethodAsync(model);

            if (!result.IsSucceeded)
            {
                return new BaseApiResponse<ApplicationUserBaseModel>(result);
            }

            //Выбрасываем событие о регистрации клиента
            await PublishMessageAsync(new ClientRegisteredEvent
            {
                UserId = result.ResponseObject.Id,
                IsPasswordGenerated = createRandomPassword,
                Password = model.Password
            });

            var user = result.ResponseObject;

            return new BaseApiResponse<ApplicationUserBaseModel>(true, "Регистрация прошла успешно.", user);
        }

        /// <summary>
        /// Метод регистрирующий пользователя администратором
        /// </summary>
        /// <param name="model"></param>
        /// <param name="roleNames"></param>
        /// <returns></returns>
        public async Task<BaseApiResponse<string>> RegisterUserByAdminAsync(RegisterModel model, string[] roleNames = null)
        {
            var validation = ValidateModelAndUserIsAdmin(model);
            
            if(!validation.IsSucceeded)
            {
                return new BaseApiResponse<string>(validation);
            }

            var result = await RegisterHelpMethodAsync(model, roleNames);

            if (!result.IsSucceeded)
            {
                return new BaseApiResponse<string>(result);
            }

            //Выбрасываем событие о регистрации клиента
            await PublishMessageAsync(new ClientRegisteredEvent
            {
                UserId = result.ResponseObject.Id
            });

            var user = result.ResponseObject;

            return new BaseApiResponse<string>(true, "Вы успешно зарегистрировали пользователя", user.Id);
        }

        private async Task<BaseApiResponse<ApplicationUserBaseModel>> RegisterHelpMethodAsync(RegisterModel model, string[] roleNames = null)
        {
            var accountSettings = GetSetting<AccountSettingsModel>();

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                EmailConfirmed = !accountSettings.ShouldUsersConfirmEmail
            };

            var checkResult = await CheckUserAsync(user);

            if (!checkResult.IsSucceeded)
            {
                return new BaseApiResponse<ApplicationUserBaseModel>(checkResult.IsSucceeded, checkResult.Message);
            }

            var creationResult = await UserManager.CreateAsync(user, model.Password);

            if (!creationResult.Succeeded)
            {
                return new BaseApiResponse<ApplicationUserBaseModel>(false, creationResult.Errors.ToList().First().Description);
            }

            if(roleNames != null && roleNames.Length > 0)
            {
                await UserManager.AddToRolesAsync(user, roleNames);
            }
            
            //Создается внутренний пользователь
            CreateHandled(new Client
            {
                Id = user.Id,
                Email = model.Email,
                Name = model.Name,
                Surname = model.Surname,
                Patronymic = model.Patronymic,
                PhoneNumber = model.PhoneNumber
            });

            var result = await TrySaveChangesAndReturnResultAsync("Пользователь создан");

            if (!result.IsSucceeded)
            {
                await RemoveUserAndClient(user.Id);

                return new BaseApiResponse<ApplicationUserBaseModel>(result);
            }

            var userModel = await UserSearcher.GetUserByIdAsync(user.Id);

            return new BaseApiResponse<ApplicationUserBaseModel>(result, userModel);
        }

        #endregion

        private async Task RemoveUserAndClient(string userId)
        {
            var userRoles = await Query<ApplicationUserRole>()
                .Where(x => x.UserId == userId).ToListAsync();

            if(userRoles.Count > 0)
            {
                DeleteHandled(userRoles);
            }

            var user = await UserManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
            
            if(user != null)
            {
                DeleteHandled(user);
                await SaveChangesAsync();
            }

            var client = await Query<Client>().FirstOrDefaultAsync(x => x.Id == userId);

            if(client != null)
            {
                DeleteHandled(client);
            }

            await SaveChangesAsync();
        }

        private async Task<BaseApiResponse> CheckUserAsync(ApplicationUser user)
        {
            if(string.IsNullOrWhiteSpace(user.Email))
            {
                return new BaseApiResponse(false, "Вы не указали адрес электронной почты");
            }

            if(string.IsNullOrWhiteSpace(user.PhoneNumber))
            {
                return new BaseApiResponse(false, "Вы не указали номер телефона");
            }

            var query = Query<ApplicationUser>();

            if (await query.AnyAsync(x => x.Email == user.Email))
            {
                return new BaseApiResponse(false, "Пользователь с данным email адресом уже существует");
            }

            if (await query.AnyAsync(x => x.PhoneNumber == user.PhoneNumber))
            {
                return new BaseApiResponse(false, "Пользователь с данным номером телефона уже существует");
            }

            return new BaseApiResponse(true, "");
        }
    }
}