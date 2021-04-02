using System.Linq;
using Clt.Contract.Models.Common;
using System.Linq.Expressions;
using System;
using Clt.Contract.Settings;
using Clt.Contract.Enumerations;
using Clt.Model.Entities.Default;

namespace Clt.Logic.Extensions
{
    internal static class ApplicationUserExtensions
    {
        public static Expression<Func<ApplicationUser, ApplicationUserBaseModel>> SelectExpression = x => new ApplicationUserBaseModel
        {
            Id = x.Id,
            Email = x.Email,
            PhoneNumber = x.PhoneNumber,
            EmailConfirmed = x.EmailConfirmed,
            PhoneNumberConfirmed = x.PhoneNumberConfirmed,
            SecurityStamp = x.SecurityStamp,
            PasswordHash = x.PasswordHash,
            RoleNames = x.Roles.Select(t => t.Role.Name).ToList()
        };

        public static bool IsInRole(this ApplicationUserBaseModel user, string roleName)
        {
            if (user.RoleNames == null)
            {
                throw new ApplicationException("Роли не установлены");
            }

            return user.RoleNames.Any(x => x == roleName);
        }

        public static UserDifferenceAction GetComparingAction(ApplicationUserBaseModel userFromCookie, ApplicationUserBaseModel userFromDb,
            AccountSettingsModel accountSettings,
            RootSettings rightsSettings)
        {
            if (!userFromCookie.EmailConfirmed && userFromCookie.Email != rightsSettings.RootEmail && !accountSettings.IsLoginEnabledForUsersWhoDidNotConfirmEmail)
            {
                return UserDifferenceAction.Logout;
            }

            if (userFromDb.DeActivated || userFromDb.PasswordHash != userFromCookie.PasswordHash)
            {
                return UserDifferenceAction.Logout;
            }

            var compareResult = Compare(userFromCookie, userFromDb);

            return !compareResult ? UserDifferenceAction.AutoReLogin : UserDifferenceAction.None;
        }


        public static bool Compare(ApplicationUserBaseModel user1, ApplicationUserBaseModel user2)
        {
            var rightsChanged = user1.RoleNames.Count != user2.RoleNames.Count;

            if (!rightsChanged)
            {
                for (var i = 0; i < user1.RoleNames.Count; i++)
                {
                    if (user1.RoleNames.OrderBy(x => x).ToList()[i] == user2.RoleNames.OrderBy(x => x).ToList()[i])
                    {
                        continue;
                    }
                    rightsChanged = true;
                    break;
                }
            }

            return user1.Id == user2.Id &&
                !rightsChanged &&
                user1.Name == user2.Name &&
                string.IsNullOrEmpty(user1.PhoneNumber) == string.IsNullOrEmpty(user2.PhoneNumber);
        }
    }
}