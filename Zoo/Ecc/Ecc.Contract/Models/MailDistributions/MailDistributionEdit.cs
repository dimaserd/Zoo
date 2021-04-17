using System.ComponentModel.DataAnnotations;

namespace Ecc.Contract.Models.MailDistributions
{
    /// <summary>
    /// Модель для редактирования рассылки
    /// </summary>
    public class MailDistributionEdit
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Название рассылки
        /// </summary>
        [Required(ErrorMessage = "Название рассылки не может быть пустым")]
        public string Name { get; set; }

        /// <summary>
        /// Заголовок сообщения
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Тело сообщения
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Отправлять всем пользователям
        /// </summary>
        public bool SendToEveryUser { get; set; }
    }
}