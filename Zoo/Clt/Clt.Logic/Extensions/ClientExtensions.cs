using Croco.Core.Application;
using Croco.Core.Common.Enumerations;
using Clt.Contract.Models.Users;
using Clt.Model.Entities;

namespace Clt.Logic.Extensions
{
    internal static class ClientExtensions
    {
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
    }
}