using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Clt.Contract.Models.Users;
using Croco.Core.Contract.Models;
using Croco.Core.Contract;
using Clt.Model.Entities;
using Croco.Core.Contract.Application;
using Clt.Contract.Resources;

namespace Clt.Logic.Services.Users
{
    /// <summary>
    /// Сервис предоставляющий методы для поиска клиентов
    /// </summary>
    public class ClientQueryService : BaseCltService
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context"></param>
        /// <param name="application"></param>
        public ClientQueryService(ICrocoAmbientContextAccessor context, ICrocoApplication application) : base(context, application)
        {
        }

        /// <summary>
        /// Получить клиента из контекста авторизации асинхронно
        /// </summary>
        /// <returns></returns>
        public async Task<BaseApiResponse<ClientModel>> GetClientFromAuthorizationAsync()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return new BaseApiResponse<ClientModel>(false, ValidationMessages.YouAreNotAuthorized);
            }

            return await GetClientByIdAsync(UserId);
        }

        /// <summary>
        /// Получить клиента из контекста авторизации
        /// </summary>
        /// <returns></returns>
        public BaseApiResponse<ClientModel> GetClientFromAuthorization()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return new BaseApiResponse<ClientModel>(false, ValidationMessages.YouAreNotAuthorized);
            }

            return GetClientById(UserId);
        }

        /// <summary>
        /// Получить клиента по идентификатору асинхронно
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseApiResponse<ClientModel>> GetClientByIdAsync(string id)
        {
            var model = await Query<Client>().FirstOrDefaultAsync(x => x.Id == id);

            return MapToResponse(model);
        }

        /// <summary>
        /// Получить клиента по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseApiResponse<ClientModel> GetClientById(string id)
        {
            var model = Query<Client>().FirstOrDefault(x => x.Id == id);

            return MapToResponse(model);
        }

        private static BaseApiResponse<ClientModel> MapToResponse(Client model)
        {
            if (model == null)
            {
                return new BaseApiResponse<ClientModel>(false, ValidationMessages.UserNotFound);
            }

            return new BaseApiResponse<ClientModel>(true, ValidationMessages.UserFound, new ClientModel
            {
                Email = model.Email,
                Name = model.Name,
                Surname = model.Surname,
                Patronymic = model.Patronymic,
                Sex = model.Sex,
                PhoneNumber = model.PhoneNumber,
                BirthDate = model.BirthDate,
                AvatarFileId = model.AvatarFileId
            });
        }
    }
}