using System.Threading.Tasks;
using Croco.Core.Contract;
using Croco.Core.Contract.Application;
using Croco.Core.Contract.Models;
using Microsoft.EntityFrameworkCore;
using Tms.Logic.Models;
using Tms.Logic.Models.Comments;
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
        public async Task<BaseApiResponse<DayTaskWithCommentsModel>> CommentDayTaskAsync(CommentDayTask model)
        {
            var validation = ValidateAuthenticationAndModel(model);

            if (!validation.IsSucceeded)
            {
                return new BaseApiResponse<DayTaskWithCommentsModel>(validation);
            }

            var repo = GetRepository<DayTask>();

            var dayTask = await repo.Query().FirstOrDefaultAsync(x => x.Id == model.DayTaskId);

            if (dayTask == null)
            {
                return new BaseApiResponse<DayTaskWithCommentsModel>(false, TaskerResource.DayTaskNotFoundByProvidedId);
            }

            var commentsRepo = GetRepository<DayTaskComment>();

            commentsRepo.CreateHandled(new DayTaskComment
            {
                AuthorId = UserId,
                Comment = model.Comment,
                DayTaskId = model.DayTaskId
            });

            return await SaveChangesAndReturnText(model.DayTaskId, TaskerResource.CommentAdded);
        }

        /// <summary>
        /// Обновить комментарий
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<BaseApiResponse<DayTaskWithCommentsModel>> UpdateDayTaskCommentAsync(UpdateDayTaskComment model)
        {
            var validation = ValidateAuthenticationAndModel(model);

            if (!validation.IsSucceeded)
            {
                return new BaseApiResponse<DayTaskWithCommentsModel>(validation);
            }

            var commentOrError = await GetCommentOrError(model.DayTaskCommentId);

            if (commentOrError.Comment == null)
            {
                return new BaseApiResponse<DayTaskWithCommentsModel>(false, commentOrError.ErrorMessage);
            }
            var comment = commentOrError.Comment;

            comment.Comment = model.Comment;

            UpdateHandled(comment);

            return await SaveChangesAndReturnText(comment.DayTaskId, TaskerResource.CommentUpdated);
        }

        /// <summary>
        /// Удаление комментария
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<BaseApiResponse<DayTaskWithCommentsModel>> DeleteDayTaskCommentAsync(DeleteDayTaskComment model)
        {
            var validation = ValidateAuthenticationAndModel(model);

            if (!validation.IsSucceeded)
            {
                return new BaseApiResponse<DayTaskWithCommentsModel>(validation);
            }

            var commentOrError = await GetCommentOrError(model.DayTaskCommentId);

            if(commentOrError.Comment == null)
            {
                return new BaseApiResponse<DayTaskWithCommentsModel>(false, commentOrError.ErrorMessage);
            }
            var comment = commentOrError.Comment;

            DeleteHandled(commentOrError.Comment);

            return await SaveChangesAndReturnText(comment.DayTaskId, TaskerResource.CommentDeleted);
        }


        private BaseApiResponse ValidateAuthenticationAndModel(object model)
        {
            if (!IsAuthenticated)
            {
                return new BaseApiResponse(false, ValidationMessages.YouAreNotAuthorized);
            }

            var validation = ValidateModel(model);

            return validation;
        }

        private Task<BaseApiResponse<DayTaskWithCommentsModel>> SaveChangesAndReturnText(string dayTaskId, string successfulText)
        {
            return TryExecuteCodeAndReturnSuccessfulResultAsync(async () =>
            {
                await SaveChangesAsync();

                var task = await DayTasksService.GetDayTaskByIdAsync(dayTaskId);

                return new BaseApiResponse<DayTaskWithCommentsModel>(true, successfulText, task);
            });
        }

        private class CommentOrError
        {
            public CommentOrError(string error)
            {
                ErrorMessage = error;
            }

            public CommentOrError(DayTaskComment comment)
            {
                Comment = comment;
            }

            public DayTaskComment Comment { get; }
            public string ErrorMessage { get; }
        }

        private async Task<CommentOrError> GetCommentOrError(string commentId)
        {
            var comment = await GetRepository<DayTaskComment>().Query().FirstOrDefaultAsync(x => x.Id == commentId);

            if (comment == null)
            {
                return new CommentOrError(TaskerResource.DayTaskCommentNotFoundById);
            }

            if (comment.AuthorId != UserId)
            {
                return new CommentOrError(TaskerResource.YouAreNotAuthorOfTaskComment);
            }

            return new CommentOrError(comment);
        }
    }
}