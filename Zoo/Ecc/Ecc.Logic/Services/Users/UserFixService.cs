using Croco.Core.Contract;
using Croco.Core.Contract.Application;
using Croco.Core.Contract.Models.Search;
using Ecc.Contract.Abstractions;
using Ecc.Logic.Models.Users;
using Ecc.Logic.Services.Base;
using Ecc.Model.Entities.External;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Ecc.Logic.Services.Users
{
    /// <summary>
    /// Сервис для проверки пользователей из мастер-хранилища
    /// </summary>
    public class UserFixService : BaseEccService
    {
        IUserMasterStorage UserMasterStorage { get; }
        UserService UserService { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context"></param>
        /// <param name="application"></param>
        /// <param name="userMasterStorage"></param>
        /// <param name="userService"></param>
        public UserFixService(ICrocoAmbientContextAccessor context, 
            ICrocoApplication application,
            IUserMasterStorage userMasterStorage,
            UserService userService) : base(context, application)
        {
            UserMasterStorage = userMasterStorage;
            UserService = userService;
        }

        /// <summary>
        /// Экспортировать пользователей
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task<UserExportResult> ExportUsers(int count)
        {
            var exportedUsersCount = await Query<EccUser>().CountAsync();

            var usersList = await UserMasterStorage.GetUsers(new GetListSearchModel
            {
                Count = count,
                OffSet = exportedUsersCount
            });

            foreach(var user in usersList.List)
            {
                await UserService.CreateOrUpdateUser(user);
            }

            return new UserExportResult
            {
                UsersExported = await Query<EccUser>().CountAsync(),
                TotalUsers = usersList.TotalCount
            };
        }
    }
}