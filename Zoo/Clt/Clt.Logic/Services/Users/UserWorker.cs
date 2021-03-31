using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Clt.Logic.Models.Users;
using System.Linq.Expressions;
using Clt.Logic.Extensions;
using Croco.Core.Contract.Models;
using Croco.Core.Contract;
using Croco.Core.Contract.Application;
using Clt.Model.Entities;
using Clt.Contract.Enumerations;
using Clt.Model.Entities.Default;
using Clt.Contract.Settings;
using Clt.Logic.Resources;
using Microsoft.Extensions.Logging;
using Clt.Contract.Events;

namespace Clt.Logic.Services.Users
{
    /// <summary>
    /// Сервис для работы с пользователями
    /// </summary>
    public class UserWorker : BaseCltService
    {
        UserSearcher UserSearcher { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="ambientContext"></param>
        /// <param name="application"></param>
        /// <param name="userSearcher"></param>
        public UserWorker(ICrocoAmbientContextAccessor ambientContext,
            ICrocoApplication application,
            UserSearcher userSearcher) : base(ambientContext, application)
        {
            UserSearcher = userSearcher;
        }

        /// <summary>
        /// Удалить пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<BaseApiResponse> RemoveUserAsync(string userId)
        {
            if (!RootSettings.UserRemovingEnabled)
            {
                return new BaseApiResponse(false, "В настройках вашего приложения выключена опция удаления пользователей");
            }

            if (!User.IsInRole(RolesSetting.RootRoleName))
            {
                return new BaseApiResponse(false, "Вы не имеете прав для удаления пользователя");
            }

            var userToRemove = await UserSearcher.GetUserByIdAsync(userId);

            if (userToRemove == null)
            {
                return new BaseApiResponse(false, "Пользователь не найден по указанному идентификатору");
            }

            if (userToRemove.IsInRole(RolesSetting.RootRoleName))
            {
                return new BaseApiResponse(false, "Вы не можете удалить Root пользователя");
            }

            await GenericDelete<Client>(x => x.Id == userId);
            await GenericDelete<ApplicationUserRole>(x => x.UserId == userId);
            await GenericDelete<ApplicationUser>(x => x.Id == userId);

            var res = await TrySaveChangesAndReturnResultAsync($"Пользователь {userToRemove.Email} удален");

            if (!res.IsSucceeded)
            {
                return res;
            }

            return res;
        }

        private async Task GenericDelete<TEntity>(Expression<Func<TEntity, bool>> whereExpression) where TEntity : class
        {
            DeleteHandled(await Query<TEntity>().Where(whereExpression).ToListAsync());
        }

        /// <summary>
        /// Редактирование пользователя администратором
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<BaseApiResponse> EditUserAsync(EditApplicationUser model)
        {
            var validation = ValidateModelAndUserIsAdmin(model);

            if (!validation.IsSucceeded)
            {
                return validation;
            }

            var clientRepo = GetRepository<Client>();

            var userDto = await UserSearcher.GetUserByIdAsync(model.Id);

            if (userDto == null)
            {
                return new BaseApiResponse(false, ValidationMessages.UserIsNotFoundByIdentifier);
            }

            if (userDto.Email == RootSettings.RootEmail)
            {
                return new BaseApiResponse(false, ValidationMessages.YouCantEditRootUser);
            }

            if (await clientRepo.Query().AnyAsync(x => x.Email == model.Email && x.Id != model.Id))
            {
                return new BaseApiResponse(false, ValidationMessages.ThisEmailIsAlreadyTaken);
            }

            if (await clientRepo.Query().AnyAsync(x => x.PhoneNumber == model.PhoneNumber && x.Id != model.Id))
            {
                return new BaseApiResponse(false, ValidationMessages.ThisPhoneNumberIsAlreadyTaken);
            }

            if (!User.IsInRole(RolesSetting.AdminRoleName) && (userDto.IsInRole(RolesSetting.AdminRoleName)))
            {
                return new BaseApiResponse(false, ValidationMessages.YouCantEditUserBecauseHeIsAdministrator);
            }

            var userToEditEntity = await clientRepo.Query().FirstOrDefaultAsync(x => x.Id == model.Id);

            if (userToEditEntity == null)
            {
                var ex = new Exception("Ужасная ошибка");

                Logger.LogError(ex, "EditUserAsync.OnException");

                return new BaseApiResponse(ex);
            }

            userToEditEntity.Email = model.Email;
            userToEditEntity.Name = model.Name;
            userToEditEntity.Surname = model.Surname;
            userToEditEntity.Patronymic = model.Patronymic;
            userToEditEntity.Sex = model.Sex;
            userToEditEntity.ObjectJson = model.ObjectJson;
            userToEditEntity.PhoneNumber = new string(model.PhoneNumber.Where(char.IsDigit).ToArray());
            userToEditEntity.BirthDate = model.BirthDate;

            clientRepo.UpdateHandled(userToEditEntity);

            var response = await TrySaveChangesAndReturnResultAsync("Данные пользователя обновлены");

            if (response.IsSucceeded)
            {
                await PublishMessageAsync(new ClientDataUpdatedEvent
                {
                    Id = userToEditEntity.Id
                });
            }

            return response;
        }

        /// <summary>
        /// Активировать или деактивировать пользователя
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<BaseApiResponse> ActivateOrDeActivateUserAsync(UserActivation model)
        {
            var userDto = await UserSearcher.GetUserByIdAsync(model.Id);

            if (userDto == null)
            {
                return new BaseApiResponse(false, "Пользователь не найден по указанному идентификатору");
            }

            var result = UserRightsExtensions.HasRightToEditUser(userDto, User, RolesSetting);

            if (!result.IsSucceeded)
            {
                return result;
            }

            var userRepo = GetRepository<Client>();

            var user = await userRepo.Query().FirstOrDefaultAsync(x => x.Id == model.Id);

            if (user == null)
            {
                return new BaseApiResponse(false, "Пользователь не найден по указанному идентификатору");
            }

            if (model.DeActivated)
            {
                if (user.DeActivated)
                {
                    return new BaseApiResponse(false, "Пользователь уже является деактивированным");
                }


                user.DeActivated = true;

                userRepo.UpdateHandled(user);

                return await TrySaveChangesAndReturnResultAsync("Пользователь деактивирован");
            }

            if (!user.DeActivated)
            {
                return new BaseApiResponse(false, "Пользователь уже активирован");
            }

            user.DeActivated = false;

            userRepo.UpdateHandled(user);

            return await TrySaveChangesAndReturnResultAsync("Пользователь активирован");
        }
    }
}