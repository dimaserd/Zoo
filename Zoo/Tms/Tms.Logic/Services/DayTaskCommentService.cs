using System.Threading.Tasks;
using Croco.Core.Contract;
using Croco.Core.Contract.Application;
using Croco.Core.Contract.Models;
using Microsoft.EntityFrameworkCore;
using Tms.Logic.Models;
using Tms.Logic.Resources;
using Tms.Model.Entities;

namespace Tms.Logic.Services
{
    /// <summary>
    /// Сервис для комментирования заданий на день
    /// </summary>
    public class DayTaskCommentService : TmsBaseService
    {
        DayTasksService DayTasksService { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="contextAccessor"></param>
        /// <param name="application"></param>
        /// <param name="principalCheker"></param>
        /// <param name="dayTasksService"></param>
        public DayTaskCommentService(ICrocoAmbientContextAccessor contextAccessor, 
            ICrocoApplication application, 
            PrincipalCheker principalCheker,
            DayTasksService dayTasksService) : base(contextAccessor, application, principalCheker)
        {
            DayTasksService = dayTasksService;
        }

        
        /// <summary>
        /// Добавить комментарий к заданию
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<BaseApiResponse<DayTaskModel>> CommentDayTaskAsync(CommentDayTask model)
        {
            if (!IsAuthenticated)
            {
                return new BaseApiResponse<DayTaskModel>(false, ValidationMessages.YouAreNotAuthorized);
            }

            var validation = ValidateModel(model);

            if (!validation.IsSucceeded)
            {
                return new BaseApiResponse<DayTaskModel>(validation);
            }

            var repo = GetRepository<DayTask>();

            var dayTask = await repo.Query().FirstOrDefaultAsync(x => x.Id == model.DayTaskId);

            if (dayTask == null)
            {
                return new BaseApiResponse<DayTaskModel>(false, TaskerResource.DayTaskNotFoundByProvidedId);
            }

            var commentsRepo = GetRepository<DayTaskComment>();

            commentsRepo.CreateHandled(new DayTaskComment
            {
                AuthorId = UserId,
                Comment = model.Comment,
                DayTaskId = model.DayTaskId
            });

            return await TryExecuteCodeAndReturnSuccessfulResultAsync(async () =>
            {
                await SaveChangesAsync();

                var task = await DayTasksService.GetDayTaskByIdAsync(model.DayTaskId);

                return new BaseApiResponse<DayTaskModel>(true, TaskerResource.CommentAdded, task);
            });
        }

        /// <summary>
        /// Обновить комментарий
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<BaseApiResponse<DayTaskModel>> UpdateDayTaskCommentAsync(UpdateDayTaskComment model)
        {
            if (!IsAuthenticated)
            {
                return new BaseApiResponse<DayTaskModel>(false, ValidationMessages.YouAreNotAuthorized);
            }

            var validation = ValidateModel(model);

            if (!validation.IsSucceeded)
            {
                return new BaseApiResponse<DayTaskModel>(validation);
            }

            var commentsRepo = GetRepository<DayTaskComment>();

            var comment = await commentsRepo.Query().FirstOrDefaultAsync(x => x.Id == model.DayTaskCommentId);

            if (comment == null)
            {
                return new BaseApiResponse<DayTaskModel>(false, TaskerResource.DayTaskCommentNotFoundById);
            }

            comment.Comment = model.Comment;

            commentsRepo.UpdateHandled(comment);

            return await TryExecuteCodeAndReturnSuccessfulResultAsync(async () =>
            {
                await SaveChangesAsync();
                var repo = GetRepository<DayTask>();

                var task = await DayTasksService.GetDayTaskByIdAsync(comment.DayTaskId);

                return new BaseApiResponse<DayTaskModel>(true, TaskerResource.CommentUpdated, task);
            });
        }
    }
}