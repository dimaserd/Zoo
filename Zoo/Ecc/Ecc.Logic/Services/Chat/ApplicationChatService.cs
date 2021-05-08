using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecc.Model.Entities.Chats;
using Ecc.Model.Entities.External;
using Ecc.Logic.Resources;
using Ecc.Contract.Models.Chat;
using Croco.Core.Contract.Models.Search;
using Croco.Core.Contract.Models;
using Croco.Core.Contract;
using Croco.Core.Contract.Application;
using Microsoft.Extensions.Logging;
using Ecc.Logic.Services.Base;
using Ecc.Contract.Events.Chat;

namespace Ecc.Logic.Services.Chat
{
    /// <summary>
    /// Сервис для работы с чатами
    /// </summary>
    public class ApplicationChatService : BaseEccService
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="ambientContext"></param>
        /// <param name="application"></param>
        public ApplicationChatService(ICrocoAmbientContextAccessor ambientContext, ICrocoApplication application) : base(ambientContext, application)
        {
        }

        private List<EccChatUserRelation> GetRelations(int chatId, string userId)
        {
            var relations = new List<EccChatUserRelation>
            {
                new EccChatUserRelation
                {
                    ChatId = chatId,
                    IsChatCreator = true,
                    UserId = UserId
                }
            };

            if (UserId != userId)
            {
                relations.Add(new EccChatUserRelation
                {
                    ChatId = chatId,
                    UserId = userId,
                });
            }

            return relations;
        }

        /// <summary>
        /// Создать диалог с пользователем
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<BaseApiResponse<int>> CreateOrGetExistingDialogWithUser(string userId)
        {
            if (!IsAuthenticated)
            {
                return new BaseApiResponse<int>(false, ValidationMessages.YouAreNotAuthorized);
            }

            if (!await Query<EccUser>().AnyAsync(x => x.Id == userId))
            {
                return new BaseApiResponse<int>(false, "Пользователь не найден по указанному идентификатору");
            }

            var result = await Query<EccChat>()
                .Where(x => x.IsDialog)
                .Where(x => x.UserRelations.Any(t => t.UserId == UserId) && x.UserRelations.Any(t => t.UserId == userId))
                .Select(x => new
                {
                    x.Id,
                }).FirstOrDefaultAsync();

            if (result != null)
            {
                return new BaseApiResponse<int>(true, "Чат с пользователем уже существует", result.Id);
            }

            var chat = new EccChat
            {
                IsDialog = true
            };

            CreateHandled(chat);

            var resp = await TrySaveChangesAndReturnResultAsync("Чат создан");

            if (!resp.IsSucceeded)
            {
                Logger.LogWarning($"ApplicationChatService.CreateOrGetExistingDialogWithUser.CretingChatError ChatCreatorUserId={UserId} userIdParam={userId}");

                return new BaseApiResponse<int>(false, $"Произошла ошибка при создании чата. {resp.Message}");
            }

            var relations = GetRelations(chat.Id, userId);

            CreateHandled(relations);

            var saveRelationsResponse = await TrySaveChangesAndReturnResultAsync("Ok");

            if (!saveRelationsResponse.IsSucceeded)
            {
                return new BaseApiResponse<int>(false, "Произошла ошибка при создании пользователей к чату, но сам чат был создан");
            }

            await PublishMessageAsync(new ChatCreatedEvent
            {
                ChatId = chat.Id,
                UserIds = relations.Select(x => x.UserId).ToList()
            });

            return new BaseApiResponse<int>(true, "Диалог с пользователем создан", chat.Id);
        }

        /// <summary>
        /// Написать сообщение
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Task<BaseApiResponse<ChatMessageModel>> SendMessage(SendMessageToChat model)
        {
            var resModel = new ChatMessageModel
            {
                Message = model.Message,
                SenderUserId = UserId,
                SentOnUtcTicks = DateTime.UtcNow.Ticks
            };

            CreateHandled(new EccChatMessage
            {
                Id = Guid.NewGuid().ToString(),
                ChatId = model.ChatId,
                SentOnUtcTicks = resModel.SentOnUtcTicks,
                Message = resModel.Message,
                SenderUserId = UserId
            });

            return TrySaveChangesAndReturnResultAsync("Сообщение отправлено", resModel);
        }

        /// <summary>
        /// Получить список непрочитанных сообщений
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetCountOfUnreadMessages()
        {
            var chatRels = await Query<EccChatUserRelation>()
                .Where(x => x.UserId == UserId)
                .Select(t => new { t.ChatId, CountOfUnread = t.Chat.Messages.Count(z => z.SentOnUtcTicks > t.LastVisitUtcTicks) })
                .ToListAsync();

            return chatRels.Sum(x => x.CountOfUnread);
        }
        
        /// <summary>
        /// Получить список чатов
        /// </summary>
        /// <returns></returns>
        public async Task<GetListResult<ChatModel>> GetChats()
        {
            var take = 50;

            var chatRels = await Query<EccChatUserRelation>()
                .Where(x => x.UserId == UserId)
                .Select(t => new { t.ChatId, t.LastVisitUtcTicks, CountOfUnread = t.Chat.Messages.Count(z => z.SentOnUtcTicks > t.LastVisitUtcTicks) })
                .ToListAsync();

            var query = Query<EccChat>().Where(x => x.UserRelations.Any(t => t.UserId == UserId)).Select(x => new ChatModel
            {
                Id = x.Id,
                ChatName = x.ChatName,
                IsDialog = x.IsDialog,
                Users = x.UserRelations.Select(t => new UserInChatModel
                {
                    LastVisitUtcTicks = t.LastVisitUtcTicks,
                    User = new Contract.Models.Users.UserIdAndEmailModel
                    {
                        Id = t.UserId,
                        Email = t.User.Email
                    }
                }).ToList(),
                LastMessage = x.Messages.OrderByDescending(t => t.SentOnUtcTicks).Select(t => new ChatMessageModel
                {
                    SenderUserId = t.SenderUserId,
                    SentOnUtcTicks = t.SentOnUtcTicks,
                    Message = t.Message
                }).FirstOrDefault()
            }).OrderByDescending(x => x.LastMessage.SentOnUtcTicks);

            var preResult = await query.Take(take).ToListAsync();

            var q = from chat in preResult
                    join chatRel in chatRels on chat.Id equals chatRel.ChatId
                    select new
                    {
                        Chat = chat,
                        chatRel.CountOfUnread
                    };

            foreach (var chat in q)
            {
                chat.Chat.CountOfUnreadMessages = chat.CountOfUnread;
            }

            return new GetListResult<ChatModel>
            {
                List = q.Select(x => x.Chat).ToList(),
                Count = take,
                OffSet = 0,
                TotalCount = await query.CountAsync()
            };
        }

        /// <summary>
        /// Посетить чат
        /// </summary>
        /// <param name="chatId"></param>
        /// <returns></returns>
        public async Task<BaseApiResponse> VisitChat(int chatId)
        {
            var chatRelation = await Query<EccChatUserRelation>()
                .FirstOrDefaultAsync(x => x.UserId == UserId && x.ChatId == chatId);

            if (chatRelation == null)
            {
                var mes = "Переязка пользователя с чатом не найдена";

                Logger.LogWarning($"ApplicationChatService.VisitChat.ChatRelationNotFound UserId={UserId} ChatId={chatId}");

                return new BaseApiResponse(false, mes);
            }

            chatRelation.LastVisitUtcTicks = DateTime.UtcNow.Ticks;

            UpdateHandled(chatRelation);

            var resp = await TrySaveChangesAndReturnResultAsync("Ok");

            if (resp.IsSucceeded)
            {
                await PublishMessageAsync(new ChatRelationUpdatedEvent
                {
                    ChatId = chatRelation.ChatId,
                    UserId = chatRelation.UserId
                });
            }

            return resp;
        }

        /// <summary>
        /// Получить сообщения
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Task<List<ChatMessageModel>> GetMessages(GetChatMessages model)
        {
            return Query<EccChatMessage>()
                .Where(x => x.ChatId == model.ChatId && x.SentOnUtcTicks <= model.LessThantUtcTicks)
                .Select(x => new ChatMessageModel
                {
                    SenderUserId = x.SenderUserId,
                    SentOnUtcTicks = x.SentOnUtcTicks,
                    Message = x.Message
                }).OrderBy(x => x.SentOnUtcTicks)
                .Take(model.Count)
                .ToListAsync();
        }
    }
}