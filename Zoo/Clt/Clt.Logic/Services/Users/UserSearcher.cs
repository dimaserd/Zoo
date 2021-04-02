using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Clt.Contract.Models.Common;
using Clt.Logic.Extensions;
using Croco.Core.Contract;
using Clt.Model.Entities.Default;
using Croco.Core.Contract.Application;

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

        private Task<ApplicationUserBaseModel> GetUserByPredicateExpression(Expression<Func<ApplicationUserBaseModel, bool>> predicate)
        {
            return Query<ApplicationUser>().Select(ApplicationUserExtensions.SelectExpression).FirstOrDefaultAsync(predicate);
        }

        #endregion

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