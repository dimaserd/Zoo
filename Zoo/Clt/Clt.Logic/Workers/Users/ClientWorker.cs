﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Clt.Logic.Resources;
using Clt.Contract.Models.Users;
using Croco.Core.Contract.Models;
using Croco.Core.Contract;
using Clt.Logic.Abstractions;
using Croco.Core.Logic.Files.Abstractions;
using Clt.Model.Entities;
using Clt.Logic.Settings;
using Clt.Model.Entities.Default;
using Croco.Core.Contract.Application;
using Clt.Logic.Core.Resources;

namespace Clt.Logic.Workers.Users
{
    public class ClientWorker : BaseCltWorker
    {
        IClientDataRefresher ClientDataRefresher { get; }
        IFileImageChecker FileImageChecker { get; }
        IDbFileManager DbFileManager { get; }

        public ClientWorker(ICrocoAmbientContextAccessor ambientContext, 
            ICrocoApplication app,
            IClientDataRefresher clientDataRefresher,
            IFileImageChecker fileImageChecker,
            IDbFileManager dbFileManager) : base(ambientContext, app)
        {
            ClientDataRefresher = clientDataRefresher;
            FileImageChecker = fileImageChecker;
            DbFileManager = dbFileManager;
        }

        private Task<ApplicationUser> GetUserByIdAsync(string id)
        {
            return Query<ApplicationUser>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BaseApiResponse> UpdateClientPhotoAsync(int fileId)
        {
            if (!IsAuthenticated)
            {
                return new BaseApiResponse(false, ValidationMessages.YouAreNotAuthorized);
            }

            var userRepo = GetRepository<Client>();
            
            var userToEditEntity = await userRepo.Query().FirstOrDefaultAsync(x => x.Id == UserId);

            if (userToEditEntity == null)
            {
                return new BaseApiResponse(false, ValidationMessages.UserNotFound);
            }

            var file = await DbFileManager.GetFileDataById(fileId);

            if (file == null)
            {
                return new BaseApiResponse(false, ValidationMessages.FileIsNotFoundById);
            }

            if (!FileImageChecker.IsImage(file))
            {
                return new BaseApiResponse(false, ValidationMessages.FileIsNotImage);
            }

            userToEditEntity.AvatarFileId = fileId;

            userRepo.UpdateHandled(userToEditEntity);

            return await TryExecuteCodeAndReturnSuccessfulResultAsync(async () =>
            {
                await SaveChangesAsync();
                await ClientDataRefresher.UpdateUserDataAsync(await GetUserByIdAsync(userToEditEntity.Id), userToEditEntity);

                return new BaseApiResponse(true, ClientResource.ClientAvatarUpdated);
            });
        }

        public async Task<BaseApiResponse> EditUserAsync(EditClient model)
        {
            if (!IsAuthenticated)
            {
                return new BaseApiResponse(false, ValidationMessages.YouAreNotAuthorized);
            }

            var validation = ValidateModel(model);

            if (!validation.IsSucceeded)
            {
                return validation;
            }

            model.PhoneNumber = new string(model.PhoneNumber.Where(char.IsDigit).ToArray());
            
            var userRepo = GetRepository<Client>();

            if (await userRepo.Query().AnyAsync(x => x.Id != UserId && x.PhoneNumber == model.PhoneNumber))
            {
                return new BaseApiResponse(false, ValidationMessages.ThisPhoneNumberIsAlreadyTakenByOtherUser);
            }

            var userToEditEntity = await userRepo.Query().FirstOrDefaultAsync(x => x.Id == UserId);

            if (userToEditEntity == null)
            {
                return new BaseApiResponse(false, ValidationMessages.UserNotFound);
            }

            if (userToEditEntity.Email == RightsSettings.RootEmail)
            {
                return new BaseApiResponse(false, "Root не может редактировать сам себя");
            }

            
            userToEditEntity.Name = model.Name;
            userToEditEntity.Surname = model.Surname;
            userToEditEntity.Patronymic = model.Patronymic;
            userToEditEntity.Sex = model.Sex;
            userToEditEntity.PhoneNumber = model.PhoneNumber;
            userToEditEntity.BirthDate = model.BirthDate;

            userRepo.UpdateHandled(userToEditEntity);

            return await TryExecuteCodeAndReturnSuccessfulResultAsync(async () =>
            {
                await RepositoryFactory.SaveChangesAsync();

                await ClientDataRefresher.UpdateUserDataAsync(await GetUserByIdAsync(userToEditEntity.Id), userToEditEntity);

                return new BaseApiResponse(true, ClientResource.ClientDataRenewed);
            });
        }
        
        public async Task<BaseApiResponse<ClientModel>> GetUserAsync()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return new BaseApiResponse<ClientModel>(false, ValidationMessages.YouAreNotAuthorized);
            }

            return await GetClientByIdAsync(UserId);
        }

        public async Task<BaseApiResponse<ClientModel>> GetClientByIdAsync(string id)
        {
            var repo = GetRepository<Client>();

            var model = await repo.Query().FirstOrDefaultAsync(x => x.Id == id);

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