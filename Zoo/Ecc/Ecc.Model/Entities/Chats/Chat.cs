using Croco.Core.Contract.Data.Entities.HaveId;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecc.Model.Entities.Chats
{
    /// <summary>
    /// Чат пользователей
    /// </summary>
    [Table(nameof(EccChat), Schema = Schemas.EccSchema)]
    public class EccChat : IHaveIntId
    {
        /// <summary>
        /// Идентификатор чата
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Является ли чат диалогои
        /// </summary>
        public bool IsDialog { get; set; }

        /// <summary>
        /// Название чата
        /// </summary>
        public string ChatName { get; set; }

        /// <summary>
        /// Сообщеня
        /// </summary>
        public virtual ICollection<EccChatMessage> Messages { get; set; }

        /// <summary>
        /// Пользователи в чате
        /// </summary>
        public virtual ICollection<EccChatUserRelation> UserRelations { get; set; }
    }
}