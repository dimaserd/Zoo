namespace Tms.Logic.Models
{
    /// <summary>
    /// Модель для получения расписания
    /// </summary>
    public class UserScheduleSearchModel
    {
        /// <summary>
        /// Сдвиг по месяцам
        /// </summary>
        public int MonthShift { get; set; }

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public string[] UserIds { get; set; }

        /// <summary>
        /// Показывать задания без исполнителя
        /// </summary>
        public bool ShowTasksWithNoAssignee { get; set; }
    }
}