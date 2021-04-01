using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Clt.Contract.Models.Common;
using Clt.Logic.Models;
using Clt.Logic.Extensions;
using Croco.Core.Search.Extensions;
using Croco.Core.Contract.Models.Search;
using Croco.Core.Contract;
using Clt.Model.Entities;
using Clt.Model.Entities.Default;
using Croco.Core.Contract.Application;
using Clt.Contract.Models.Users;
using System.Collections.Generic;

namespace Clt.Logic.Services.Users
{
    /// <summary>
    /// Класс предоставляющий методы для поиска пользователей
    /// </summary>
    public class UserSearcher : BaseCltService
    {
        #region Методы получения одного пользователя

        /// <summary>
        /// Найти пользователя по номеру телефона
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public Task<ApplicationUserBaseModel> GetUserByPhoneNumberAsync(string phoneNumber)
        {
            return GetUserByPredicateExpression(x => x.PhoneNumber == phoneNumber);
        }

        /// <summary>
        /// Найти пользователя по идентификатору
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<ApplicationUserBaseModel> GetUserByIdAsync(string userId)
        {   
            return GetUserByPredicateExpression(x => x.Id == userId);
        }

        /// <summary>
        /// Найти пользователя по Email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public Task<ApplicationUserBaseModel> GetUserByEmailAsync(string email)
        {
            return GetUserByPredicateExpression(x => x.Email == email);
        }

        private IQueryable<ClientJoinedWithApplicationUser> GetInitialJoinedQuery()
        {
            return ClientExtensions
                .GetInitialJoinedQuery(Query<ApplicationUser>(), Query<Client>());
        }

        private Task<ApplicationUserBaseModel> GetUserByPredicateExpression(Expression<Func<ApplicationUserBaseModel, bool>> predicate)
        {
            return GetInitialJoinedQuery().Select(ClientExtensions.SelectExpression).FirstOrDefaultAsync(predicate);
        }

        #endregion

        #region Метод получения списка пользователей

        /// <summary>
        /// Искать пользователей
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Task<GetListResult<UserWithNameAndEmailAvatarModel>> GetUsers(UserSearch model)
        {
            var criterias = GetCriterias(model);

            var clientQuery = Query<Client>().BuildQuery(criterias)
                .OrderByDescending(x => x.CreatedOn);

            return EFCoreExtensions.GetAsync(model, clientQuery, x => new UserWithNameAndEmailAvatarModel
            {
                Id = x.Id,
                Email = x.Email,
                Name = x.Name,
                AvatarFileId = x.AvatarFileId
            });
        }

        /// <summary>
        /// Получить пользователей
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Task<GetListResult<ApplicationUserBaseModel>> GetUsersAsync(UserSearch model)
        {
            var criterias = GetCriterias(model);

            var clientQuery = Query<Client>().BuildQuery(criterias);

            var q = ClientExtensions.GetInitialJoinedQuery(Query<ApplicationUser>(), clientQuery).OrderByDescending(x => x.Client.CreatedOn);

            return EFCoreExtensions.GetAsync(model, q, ClientExtensions.SelectExpression);
        }
        #endregion

        internal IEnumerable<SearchQueryCriteria<Client>> GetCriterias(UserSearch model)
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
        /// Конструктор
        /// </summary>
        /// <param name="ambientContext"></param>
        /// <param name="app"></param>
        public UserSearcher(ICrocoAmbientContextAccessor ambientContext, ICrocoApplication app) : base(ambientContext, app)
        {
        }
    }
}