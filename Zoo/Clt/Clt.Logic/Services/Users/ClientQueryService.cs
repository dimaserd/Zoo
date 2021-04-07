using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Clt.Contract.Models.Users;
using Croco.Core.Contract.Models;
using Croco.Core.Contract;
using Clt.Model.Entities;
using Croco.Core.Contract.Application;
using Clt.Contract.Resources;
using System.Linq.Expressions;
using System;
using Croco.Core.Contract.Models.Search;
using System.Collections.Generic;
using Clt.Contract.Models.Clients;
using Croco.Core.Search.Extensions;
using Clt.Model.Entities.Default;

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
        /// Получить роли клиента
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public Task<string[]> GetClientRolesAsync(string clientId)
        {
            return Query<ApplicationUserRole>()
                .Where(x => x.UserId == clientId)
                .Select(x => x.Role.Name)
                .ToArrayAsync();
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
            var model = await Query<Client>().Select(ClientSelectExpression).FirstOrDefaultAsync(x => x.Id == id);

            return MapToResponse(model);
        }

        /// <summary>
        /// Искать клиентов
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Task<GetListResult<ClientModel>> GetClients(ClientSearch model)
        {
            var criterias = GetCriterias(model);

            var clientQuery = Query<Client>().BuildQuery(criterias)
                .OrderByDescending(x => x.CreatedOn);

            return EFCoreExtensions.GetAsync(model, clientQuery, ClientSelectExpression);
        }


        internal IEnumerable<SearchQueryCriteria<Client>> GetCriterias(ClientSearch model)
        {
            yield return model.Q.MapString(str => new SearchQueryCriteria<Client>(x => x.Email.Contains(str) || x.PhoneNumber.Contains(str) || x.Name.Contains(str)));

            yield return model.Deactivated.MapNullable(b => new SearchQueryCriteria<Client>(x => x.DeActivated == b));

            yield return model.RegistrationDate.GetSearchCriteriaFromGenericRange<Client, DateTime>(x => x.CreatedOn);

            if (model.SearchSex)
            {
                yield return new SearchQueryCriteria<Client>(x => x.Sex == model.Sex);
            }
        }

        /// <summary>
        /// Получить клиента по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseApiResponse<ClientModel> GetClientById(string id)
        {
            var model = Query<Client>().Select(ClientSelectExpression).FirstOrDefault(x => x.Id == id);

            return MapToResponse(model);
        }

        private static BaseApiResponse<ClientModel> MapToResponse(ClientModel model)
        {
            if (model == null)
            {
                return new BaseApiResponse<ClientModel>(false, ValidationMessages.UserNotFound);
            }

            return new BaseApiResponse<ClientModel>(true, ValidationMessages.UserFound, model);
        }

        internal static Expression<Func<Client, ClientModel>> ClientSelectExpression = model => new ClientModel
        {
            Id = model.Id,
            Email = model.Email,
            Name = model.Name,
            Surname = model.Surname,
            Patronymic = model.Patronymic,
            Sex = model.Sex,
            PhoneNumber = model.PhoneNumber,
            BirthDate = model.BirthDate,
            AvatarFileId = model.AvatarFileId
        };
    }
}