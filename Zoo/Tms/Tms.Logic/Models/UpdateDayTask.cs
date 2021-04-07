namespace Tms.Logic.Models
{
    /// <summary>
    /// Создать или обновить задание
    /// </summary>
    public class UpdateDayTask
    {
        /// <summary>
        /// Идентификатор задания
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Данные в задании
        /// </summary>
        public DayTaskPayload Payload { get; set; }
    }
}