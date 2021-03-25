using Newtonsoft.Json;

namespace Clt.Model.Entities
{
    /// <summary>
    /// Сущность описывающая приглашение одного пользователя другим
    /// </summary>
    public class UserInvitation
    {
        /// <summary>
        /// Идентификатор приглашения
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Идентификатор клиента отправителя
        /// </summary>
        public string SenderUserId { get; set; }

        /// <summary>
        /// Клиент отправитель
        /// </summary>
        [JsonIgnore]
        public virtual Client SenderUser { get; set; }

        /// <summary>
        /// Идентификатор клиента получателя
        /// </summary>
        public string ReceiverUserId { get; set; }

        /// <summary>
        /// Клиент получатель
        /// </summary>
        [JsonIgnore]
        public virtual Client ReceiverUser { get; set; }

        /// <summary>
        /// Адрес электронной почты
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Номер телефона
        /// </summary>
        public string PhoneNumber { get; set; }
    }
}