using System;

namespace Zoo.ServerJs.Models
{
    /// <summary>
    /// Данные для исключения
    /// </summary>
    public class ExcepionData
    {
        internal static ExcepionData Create(Exception ex)
        {
            return new ExcepionData
            {
                Message = ex.Message,
                StackTrace = ex.StackTrace
            };
        }

        /// <summary>
        /// Сообщение
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Стек вызовов
        /// </summary>
        public string StackTrace { get; set; }
    }
}