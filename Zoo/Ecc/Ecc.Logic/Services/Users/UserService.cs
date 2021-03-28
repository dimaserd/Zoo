using Croco.Core.Contract;
using Croco.Core.Contract.Application;
using Croco.Core.Contract.Models;
using Ecc.Contract.Commands;
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
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context"></param>
        /// <param name="application"></param>
        public UserService(ICrocoAmbientContextAccessor context, ICrocoApplication application) : base(context, application)
        {
        }

        /// <summary>
        /// Создать пользователя
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<BaseApiResponse> CreateUser(CreateUserCommand model)
        {
            var repo = GetRepository<EccUser>();

            if(await repo.Query().AnyAsync(x => x.Id == model.UserId))
            {
                return new BaseApiResponse(false, "Пользователь с таким идентификатором уже существует");
            }

            repo.CreateHandled(new EccUser
            {
                Id = model.UserId,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber
            });

            return await TrySaveChangesAndReturnResultAsync("Пользователь успешно добавлен");
        }

        /// <summary>
        /// Обновить пользователя
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<BaseApiResponse> UpdateUser(UpdateUserCommand model)
        {
            var repo = GetRepository<EccUser>();

            var user = await repo.Query().FirstOrDefaultAsync(x => x.Id == model.UserId);

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