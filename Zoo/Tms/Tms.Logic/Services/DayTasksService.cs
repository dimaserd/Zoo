using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Croco.Core.Contract;
using Croco.Core.Contract.Application;
using Croco.Core.Contract.Models;
using Microsoft.EntityFrameworkCore;
using Tms.Logic.Abstractions;
using Tms.Logic.Models;
using Tms.Model.Entities;

namespace Tms.Logic.Services
{
    /// <summary>
    /// Класс для работы с заданиями на день 
    /// </summary>
    public class DayTasksService : TmsBaseService
    {
        /// <summary>
        /// Задания можно создавать и редактировать за один прошедший день
        /// </summary>
        public int DaysSpan = 1;

        IUsersStorage UsersStorage { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="contextAccessor"></param>
        /// <param name="application"></param>
        /// <param name="principalCheker"></param>
        /// <param name="usersStorage"></param>
        public DayTasksService(ICrocoAmbientContextAccessor contextAccessor, 
            ICrocoApplication application, 
            PrincipalCheker principalCheker,
            IUsersStorage usersStorage) : base(contextAccessor, application, principalCheker)
        {
            UsersStorage = usersStorage;
        }

        
        /// <summary>
        /// Возвращает задания на месяц для текущего пользователя
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<DayTaskModel[]> GetDayTasksAsync(UserScheduleSearchModel model)
        {
            if (model == null)
            {
                return null;
            }

            var dateNow = DateTime.Now;

            var tuple = GetDatesTuple(dateNow, model.MonthShift);

            var firstDayInSearchMonth = tuple.Item1;

            var lastDayInSearchMonth = tuple.Item2;

            var initQuery = Query<DayTask>()
                .Where(x => x.TaskDate >= firstDayInSearchMonth && x.TaskDate <= lastDayInSearchMonth);

            var noAssignee = model.ShowTasksWithNoAssignee || model.UserIds == null || model.UserIds.Length == 0;

            initQuery = noAssignee ?
                initQuery.Where(x => x.AssigneeUserId == null)
                : initQuery.Where(x => model.UserIds.Contains(x.AssigneeUserId));

            var result = await initQuery.Select(SelectExpression)
                .ToListAsync();

            return await GetDayTasks(result);
        }

        /// <summary>
        /// Получить задание по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<DayTaskModel> GetDayTaskByIdAsync(string id)
        {
            var res = await Query<DayTask>()
                .Select(SelectExpression)
                .FirstOrDefaultAsync(x => x.Id == id);

            return await GetDayTask(res);
        }

        
        /// <summary>
        /// Создание задания
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<BaseApiResponse> CreateDayTaskAsync(DayTaskPayload model)
        {
            if (model.TaskDate < GetAllowedDate())
            {
                return new BaseApiResponse(false, "Вы не можете создавать задания на прошедшую дату");
            }

            if (string.IsNullOrWhiteSpace(model.TaskTitle))
            {
                return new BaseApiResponse(false, "Пустое название задания");
            }

            if (!IsAuthenticated)
            {
                return new BaseApiResponse(false, "Вы не авторизованы");
            }

            if (string.IsNullOrWhiteSpace(model.TaskText))
            {
                return new BaseApiResponse(false, "Пустое описание задания");
            }

            if ((await UsersStorage.GetUserById(model.AssigneeUserId)) == null)
            {
                return new BaseApiResponse(false, "Пользователь не найден по указанному идентификатору");
            }

            var repo = GetRepository<DayTask>();

            repo.CreateHandled(new DayTask
            {
                AuthorId = UserId,
                AssigneeUserId = model.AssigneeUserId,
                TaskDate = model.TaskDate,
                TaskText = model.TaskText,
                TaskTitle = model.TaskTitle,
                TaskComment = model.TaskComment,
                TaskReview = model.TaskReview,
                TaskTarget = model.TaskTarget
            });

            return await TrySaveChangesAndReturnResultAsync("Задание создано");
        }

        /// <summary>
        /// Редактирование задания
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<BaseApiResponse> UpdateDayTaskAsync(UpdateDayTask model)
        {
            var userId = UserId;

            var payLoad = model.Payload;

            if(payLoad == null)
            {
                return new BaseApiResponse(false, "");
            }

            if (string.IsNullOrWhiteSpace(payLoad.TaskTitle))
            {
                return new BaseApiResponse(false, "Пустое название задания");
            }

            if (string.IsNullOrWhiteSpace(payLoad.TaskText))
            {
                return new BaseApiResponse(false, "Пустое описание задания");
            }

            var repo = GetRepository<DayTask>();

            var dayTask = await repo.Query().FirstOrDefaultAsync(x => x.Id == model.Id);

            if (dayTask == null)
            {
                return new BaseApiResponse(false, "Задание не найдено по указанному идентификатору");
            }

            if (dayTask.AssigneeUserId != userId && !IsUserAdmin())
            {
                return new BaseApiResponse(false, "Вы не можете редактировать данное задание, так как вы не являетесь его исполнителем");
            }

            dayTask.TaskTitle = payLoad.TaskTitle;
            dayTask.TaskText = payLoad.TaskText;
            dayTask.TaskDate = payLoad.TaskDate;
            dayTask.TaskComment = payLoad.TaskComment;
            dayTask.TaskReview = payLoad.TaskReview;
            dayTask.TaskTarget = payLoad.TaskTarget;

            repo.UpdateHandled(dayTask);

            return await TrySaveChangesAndReturnResultAsync("Задание обновлено");
        }

        private async Task<DayTaskModel> GetDayTask(DayTaskModelWithNoUsersJustIds model)
        {
            var users = await UsersStorage.GetUsersDictionary();

            return ToDayTaskModel(users, model);
        }

        private async Task<DayTaskModel[]> GetDayTasks(List<DayTaskModelWithNoUsersJustIds> model)
        {
            var users = await UsersStorage.GetUsersDictionary();

            return model
                .Select(m => ToDayTaskModel(users, m))
                .ToArray();
        }

        private DayTaskModel ToDayTaskModel(Dictionary<string, UserFullNameEmailAndAvatarModel> users,
            DayTaskModelWithNoUsersJustIds model)
        {
            users.TryGetValue(model.AuthorId, out var author);
            users.TryGetValue(model.AssigneeId, out var assignee);
            return new DayTaskModel(model, author, assignee);
        }

        internal static Expression<Func<DayTask, DayTaskModelWithNoUsersJustIds>> SelectExpression = x => new DayTaskModelWithNoUsersJustIds
        {
            Id = x.Id,
            TaskTitle = x.TaskTitle,
            TaskReview = x.TaskReview,
            TaskText = x.TaskText,
            FinishDate = x.FinishDate,
            TaskDate = x.TaskDate,
            TaskComment = x.TaskComment,
            TaskTarget = x.TaskTarget,

            AuthorId = x.AuthorId,
            AssigneeId = x.AssigneeUserId
        };


        /// <summary>
        /// Получить допустимую дату
        /// </summary>
        /// <returns></returns>
        private DateTime GetAllowedDate()
        {
            var dateNow = DateTime.Now;

            var todayDate = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day);

            return todayDate.AddDays(-DaysSpan);
        }


        /// <summary>
        /// Метод высчитывающий начальную 
        /// </summary>
        /// <param name="dateNow"></param>
        /// <param name="monthShift"></param>
        /// <returns></returns>
        private Tuple<DateTime, DateTime> GetDatesTuple(DateTime dateNow, int monthShift)
        {
            var date = new DateTime(dateNow.Year, dateNow.Month, 1);

            var dateMonthShifted = date.AddMonths(monthShift);

            var firstDayInSearchMonth = new DateTime(dateMonthShifted.Year, dateMonthShifted.Month, 1);

            var lastDayInSearchMonth = new DateTime(dateMonthShifted.Year, dateMonthShifted.Month, DateTime.DaysInMonth(dateMonthShifted.Year, dateMonthShifted.Month));

            return new Tuple<DateTime, DateTime>(firstDayInSearchMonth, lastDayInSearchMonth);
        }
    }
}