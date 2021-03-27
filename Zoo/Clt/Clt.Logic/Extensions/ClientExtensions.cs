using Croco.Core.Application;
using Croco.Core.Common.Enumerations;
using System.Linq;
using Clt.Contract.Models.Common;
using System.Linq.Expressions;
using Clt.Logic.Models;
using System;
using Clt.Contract.Models.Users;
using Clt.Contract.Settings;
using System.Collections.Generic;
using Clt.Model.Entities;
using Clt.Contract.Enumerations;
using Clt.Model.Entities.Default;

namespace Clt.Logic.Extensions
{
    internal static class ClientExtensions
    {
        public static Expression<Func<ClientJoinedWithApplicationUser, ApplicationUserBaseModel>> SelectExpression = x => new ApplicationUserBaseModel
        {
            Name = x.Client.Name,
            Surname = x.Client.Surname,
            ObjectJson = x.Client.ObjectJson,
            DeActivated = x.Client.DeActivated,
            BirthDate = x.Client.BirthDate,
            Patronymic = x.Client.Patronymic,
            Sex = x.Client.Sex,
            AvatarFileId = x.Client.AvatarFileId,
            CreatedOn = x.Client.CreatedOn,
            Id = x.User.Id,
            Email = x.User.Email,
            PhoneNumber = x.User.PhoneNumber,
            EmailConfirmed = x.User.EmailConfirmed,
            PhoneNumberConfirmed = x.User.PhoneNumberConfirmed,
            SecurityStamp = x.User.SecurityStamp,
            PasswordHash = x.User.PasswordHash,  
            RoleNames = x.User.Roles.Select(t => t.Role.Name).ToList()
        };

        public static IQueryable<ClientJoinedWithApplicationUser> GetInitialJoinedQuery(IQueryable<ApplicationUser> usersQuery, IQueryable<Client> clientsQuery)
        {
            return from u in usersQuery
                   join c in clientsQuery on u.Id equals c.Id
                   select new ClientJoinedWithApplicationUser
                   {
                       User = u,
                       Client = c
                   };
        }

        public static bool HasRight(this ApplicationUserBaseModel user, UserRight right)
        {
            if (user.RoleNames == null)
            {
                throw new ApplicationException("Роли не установлены");
            }

            return user.RoleNames.Any(x => x == right.ToString());
        }

        public static List<UserRight> GetRights(this ApplicationUserBaseModel user)
        {
            return user.RoleNames.Select(x =>
            {
                UserRight? res = null;

                if (Enum.TryParse(x, out UserRight result))
                {
                    res = result;
                }

                return res;
            }).Where(x => x.HasValue)
            .Select(x => x.Value)
            .ToList();
        }

        public static string GetAvatarLink(this ClientModel user, ImageSizeType imageSizeType)
        {
            var imageId = user?.AvatarFileId;

            return imageId.HasValue ? CrocoApp.Application.FileCopyWorker.GetVirtualResizedImageLocalPath(imageId.Value, imageSizeType) : null;
        }

        public static string GetAvatarLink(this Client user, ImageSizeType imageSizeType)
        {
            var imageId = user?.AvatarFileId;

            return imageId.HasValue ? CrocoApp.Application.FileCopyWorker.GetVirtualResizedImageLocalPath(imageId.Value, imageSizeType) : null;
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
                user1.AvatarFileId == user2.AvatarFileId &&
                string.IsNullOrEmpty(user1.ObjectJson) == string.IsNullOrEmpty(user2.ObjectJson) &&
                string.IsNullOrEmpty(user1.PhoneNumber) == string.IsNullOrEmpty(user2.PhoneNumber);
        }
    }
}