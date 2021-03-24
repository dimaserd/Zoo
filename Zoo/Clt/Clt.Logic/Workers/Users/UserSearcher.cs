using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Clt.Logic.Models.Users;
using Clt.Contract.Models.Common;
using Clt.Logic.Models;
using Clt.Logic.Extensions;
using System.Collections.Generic;
using Croco.Core.Search.Extensions;
using Croco.Core.Contract.Models.Search;
using Croco.Core.Contract;
using Clt.Model.Entities;
using Clt.Model.Entities.Default;
using Croco.Core.Contract.Application;

namespace Clt.Logic.Workers.Users
{
    /// <summary>
    /// Класс предоставляющий методы для поиска пользователей
    /// </summary>
    public class UserSearcher : BaseCltWorker
    {
        #region Методы получения одного пользователя

        public Task<ApplicationUserBaseModel> GetUserByPhoneNumberAsync(string phoneNumber)
        {
            return GetUserByPredicateExpression(x => x.PhoneNumber == phoneNumber);
        }

        public Task<ApplicationUserBaseModel> GetUserByIdAsync(string userId)
        {   
            return GetUserByPredicateExpression(x => x.Id == userId);
        }


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

        public static IEnumerable<string> JoinLists(IEnumerable<string> list1, IEnumerable<string> list2)
        {
            return from l1 in list1
                   join l2 in list2 on l1 equals l2
                   select l1;
        }

        public Task<GetListResult<UserWithNameAndEmailAvatarModel>> GetUsers(UserSearch model)
        {
            var criterias = model.GetCriterias();

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

        public Task<GetListResult<ApplicationUserBaseModel>> GetUsersAsync(UserSearch model)
        {
            var criterias = model.GetCriterias();

            var clientQuery = Query<Client>().BuildQuery(criterias);

            var q = ClientExtensions.GetInitialJoinedQuery(Query<ApplicationUser>(), clientQuery).OrderByDescending(x => x.Client.CreatedOn);

            return EFCoreExtensions.GetAsync(model, q, ClientExtensions.SelectExpression);
        }
        #endregion

        public UserSearcher(ICrocoAmbientContextAccessor ambientContext, ICrocoApplication app) : base(ambientContext, app)
        {
        }
    }
}