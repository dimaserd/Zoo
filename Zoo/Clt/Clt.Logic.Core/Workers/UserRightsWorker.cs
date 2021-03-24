using Clt.Contract.Models.Common;
using Clt.Contract.Settings;
using Croco.Core.Contract.Models;
using System.Linq;
using System.Security.Principal;

namespace Clt.Logic.Core.Workers
{
    public static class UserRightsWorker
    {
        public static BaseApiResponse HasRightToEditUser(ApplicationUserBaseModel userDto, IPrincipal userPrincipal, CltRolesSetting setting)
        {
            if (userPrincipal.IsInRole(setting.RootRoleName))
            {
                return new BaseApiResponse(true, "Root может делать все что угодно");
            }

            if (userDto.RoleNames.Any(x => x == setting.RootRoleName))
            {
                return new BaseApiResponse(false, "Вы не можете редактировать пользователя Root");
            }

            var isAdmin = userPrincipal.IsInRole(setting.AdminRoleName);

            if (isAdmin && userDto.RoleNames.Any(x => x == setting.AdminRoleName))
            {
                return new BaseApiResponse(false, "Вы не можете редактировать пользователя, который является Администратором, для этого нужны права Супер-Администратора");
            }

            if (isAdmin)
            {
                return new BaseApiResponse(true, "Вы не можете редактировать пользователя, который является Администратором, для этого нужны права Супер-Администратора");
            }

            return new BaseApiResponse(true, "Вы не можете редактировать пользователя, так как у вас недостаточно прав");
        }
    }
}