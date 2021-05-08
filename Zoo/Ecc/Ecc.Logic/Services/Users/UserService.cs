using Croco.Core.Contract;
using Croco.Core.Contract.Application;
using Croco.Core.Contract.Models;
using Ecc.Contract.Abstractions;
using Ecc.Contract.Models.Users;
using Ecc.Logic.Services.Base;
using Ecc.Model.Entities.External;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Ecc.Logic.Services.Users
{
    /// <summary>
    /// Сервис для работы с пользователями
    /// </summary>
    public class UserService : BaseEccService
    {
        IUserMasterStorage UserMasterStorage { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context"></param>
        /// <param name="application"></param>
        /// <param name="userMasterStorage"></param>
        public UserService(ICrocoAmbientContextAccessor context, ICrocoApplication application,
            IUserMasterStorage userMasterStorage) : base(context, application)
        {
            UserMasterStorage = userMasterStorage;
        }

        /// <summary>
        /// Создать пользователя
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseApiResponse> CreateUser(string id)
        {
            var user = await UserMasterStorage.GetUserById(id);

            if(user == null)
            {
                return new BaseApiResponse(false, "Пользователь не найден в мастер-хранилище по указанному идентификатору");
            }

            return await CreateUserInner(user);
        }

        /// <summary>
        /// Обновить пользователя
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseApiResponse> UpdateUser(string id)
        {
            var user = await UserMasterStorage.GetUserById(id);

            if (user == null)
            {
                return new BaseApiResponse(false, "Пользователь не найден в мастер-хранилище по указанному идентификатору");
            }

            return await UpdateUserInner(user);
        }

        /// <summary>
        /// Создать пользователя внутренний метод
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<BaseApiResponse> CreateUserInner(EccUserModel model)
        {
            var repo = GetRepository<EccUser>();

            if(await repo.Query().AnyAsync(x => x.Id == model.Id))
            {
                return new BaseApiResponse(false, "Пользователь с таким идентификатором уже существует");
            }

            repo.CreateHandled(new EccUser
            {
                Id = model.Id,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber
            });

            return await TrySaveChangesAndReturnResultAsync("Пользователь успешно добавлен");
        }

        /// <summary>
        /// Обновить пользователя внутренний метод
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<BaseApiResponse> UpdateUserInner(EccUserModel model)
        {
            var repo = GetRepository<EccUser>();

            var user = await repo.Query().FirstOrDefaultAsync(x => x.Id == model.Id);

            if (user == null)
            {
                return new BaseApiResponse(false, "Пользователь не найден с таким идентификатором");
            }

            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;

            repo.UpdateHandled(user);

            return await TrySaveChangesAndReturnResultAsync("Пользователь успешно добавлен");
        }
    }
}