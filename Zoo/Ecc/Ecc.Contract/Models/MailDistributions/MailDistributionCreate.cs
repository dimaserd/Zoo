using System.ComponentModel.DataAnnotations;

namespace Ecc.Contract.Models.MailDistributions
{
    /// <summary>
    /// Создание рассылки
    /// </summary>
    public class MailDistributionCreate
    {
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
        /// Отправлять каждому пользователю
        /// </summary>
        public bool SendToEveryUser { get; set; }
    }
}