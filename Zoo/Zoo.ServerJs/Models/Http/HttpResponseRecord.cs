using System;
using Zoo.ServerJs.Statics;

namespace Zoo.ServerJs.Models.Http
{
    /// <summary>
    /// Запись об Http ответе
    /// </summary>
    public class HttpResponseRecord
    {
        /// <summary>
        /// Название хоста
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// Урл хоста
        /// </summary>
        public string HostUrl { get; set; }

        /// <summary>
        /// Сериализованный запрос
        /// </summary>
        public string Request { get; set; }

        /// <summary>
        /// Урл заголовка
        /// </summary>
        public string RequestUrl { get; set; }

        /// <summary>
        /// Метод запроса
        /// </summary>
        public string RequestMethod { get; set; }

        /// <summary>
        /// Данные о возникшем исключении
        /// </summary>
        public ExcepionData ExcepionData { get; set; }

        /// <summary>
        /// Http статус код ответа
        /// </summary>
        public int ResponseStatusCode { get; set; }

        /// <summary>
        /// Сериализованный ответ
        /// </summary>
        public string Response { get; set; }

        internal TResult GetResult<TResult>()
        {
            if (!IsSuccessfull())
            {
                throw new Exception("Ответ не является успешным");
            }

            return ZooSerializer.Deserialize<TResult>(Response);
        }

        internal bool IsSuccessfull()
        {
            return ResponseStatusCode == 200;
        }
    }
}