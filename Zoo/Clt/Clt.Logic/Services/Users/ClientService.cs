﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Croco.Core.Contract.Models;
using Croco.Core.Contract;
using Clt.Logic.Abstractions;
using Croco.Core.Logic.Files.Abstractions;
using Clt.Model.Entities;
using Clt.Model.Entities.Default;
using Croco.Core.Contract.Application;
using Clt.Contract.Settings;
using Clt.Contract.Events;
using Clt.Contract.Resources;
using Croco.Core.Logic.Files.Services;
using Clt.Contract.Models.Clients;
using Croco.Core.Logic.Files.Models;

namespace Clt.Logic.Services.Users
{
    /// <summary>
    /// Менеджер для работы с клиентами
    /// </summary>
    public class ClientService : BaseCltService
    {
        const string ClientAvatarRelationName = "Clt.ClientAvatar";

        IClientDataRefresher ClientDataRefresher { get; }
        FileChecker FileChecker { get; }
        IDbFileManager DbFileManager { get; }
        IDbFileRelationManager DbFileRelationManager { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="ambientContext"></param>
        /// <param name="app"></param>
        /// <param name="clientDataRefresher"></param>
        /// <param name="fileChecker"></param>
        /// <param name="dbFileManager"></param>
        /// <param name="relationManager"></param>
        public ClientService(ICrocoAmbientContextAccessor ambientContext, 
            ICrocoApplication app,
            IClientDataRefresher clientDataRefresher,
            FileChecker fileChecker,
            IDbFileManager dbFileManager,
            IDbFileRelationManager relationManager) : base(ambientContext, app)
        {
            ClientDataRefresher = clientDataRefresher;
            FileChecker = fileChecker;
            DbFileManager = dbFileManager;
            DbFileRelationManager = relationManager;
        }

        private Task<ApplicationUser> GetUserByIdAsync(string id)
        {
            return Query<ApplicationUser>().FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Удалить аватар клиента
        /// </summary>
        /// <returns></returns>
        public async Task<BaseApiResponse> RemoveClientAvatarAsync()
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

            var oldFileId = userToEditEntity.AvatarFileId;

            userToEditEntity.AvatarFileId = null;
            userRepo.UpdateHandled(userToEditEntity);

            return await TryExecuteCodeAndReturnSuccessfulResultAsync(async () =>
            {
                await SaveChangesAsync();

                if (oldFileId.HasValue)
                {
                    await DbFileRelationManager.DeleteFileRelation(new DeleteFileRelation<Client>
                    {
                        FileId = oldFileId.Value,
                        RelationName = ClientAvatarRelationName,
                        EntityKey = userToEditEntity.Id
                    });
                }
                
                await PublishMessageAsync(new ClientDataUpdatedEvent
                {
                    Id = UserId
                });
                await ClientDataRefresher.UpdateUserDataAsync(await GetUserByIdAsync(userToEditEntity.Id), userToEditEntity);

                return new BaseApiResponse(true, ClientResource.ClientAvatarUpdated);
            });
        }

        /// <summary>
        /// Обновить фото клиента
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public async Task<BaseApiResponse> UpdateClientAvatarAsync(int fileId)
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

            if (!FileChecker.IsImage(file))
            {
                return new BaseApiResponse(false, ValidationMessages.FileIsNotImage);
            }

            var oldFileId = userToEditEntity.AvatarFileId;
            userToEditEntity.AvatarFileId = fileId;

            userRepo.UpdateHandled(userToEditEntity);

            return await TryExecuteCodeAndReturnSuccessfulResultAsync(async () =>
            {
                await SaveChangesAsync();

                await DbFileRelationManager.AddOrUpdateFileRelation(new AddOrUpdateFileRelation<Client>
                {
                    FileId = fileId,
                    EntityKey = userToEditEntity.Id,
                    RelationCustomData = null,
                    RelationName = ClientAvatarRelationName,
                    RelationValue = userToEditEntity.Id
                });

                await PublishMessageAsync(new ClientDataUpdatedEvent
                {
                    Id = UserId
                });
                await ClientDataRefresher.UpdateUserDataAsync(await GetUserByIdAsync(userToEditEntity.Id), userToEditEntity);

                return new BaseApiResponse(true, ClientResource.ClientAvatarUpdated);
            });
        }

        /// <summary>
        /// Редактировать клиента
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<BaseApiResponse> EditClientAsync(EditClient model)
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

            if (userToEditEntity.Email == RootSettings.RootEmail)
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

                await PublishMessageAsync(new ClientDataUpdatedEvent
                {
                    Id = userToEditEntity.Id
                });

                return new BaseApiResponse(true, ClientResource.ClientDataRenewed);
            });
        }
    }
}