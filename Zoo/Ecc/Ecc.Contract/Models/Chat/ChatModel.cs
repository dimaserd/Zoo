using System.Collections.Generic;
using System.ComponentModel;

namespace Ecc.Contract.Models.Chat
{
    public class ChatModel
    {
        /// <summary>
        /// Идентификатор чата
        /// </summary>
        [Description("Идентификатор чата")]
        public int Id { get; set; }

        /// <summary>
        /// Является ли данный чат диалогом
        /// </summary>
        [Description("Является ли данный чат диалогом")]
        public bool IsDialog { get; set; }

        /// <summary>
        /// Кол-во непрочитанных сообщений
        /// </summary>
        public int CountOfUnreadMessages { get; set; }

        /// <summary>
        /// Название чата
        /// </summary>
        [Description("Название чата")]
        public string ChatName { get; set; }

        /// <summary>
        /// Пользователи в чате
        /// </summary>
        [Description("Пользователи в чате")]
        public List<UserInChatModel> Users { get; set; }

        /// <summary>
        /// Последнее отправленное сообщение
        /// </summary>
        [Description("Последнее отправленное сообщение")]
        public ChatMessageModel LastMessage { get; set; }
    }
}