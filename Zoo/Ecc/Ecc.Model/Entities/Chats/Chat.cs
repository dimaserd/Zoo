using Croco.Core.Contract.Data.Entities.HaveId;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecc.Model.Entities.Chats
{
    [Table(nameof(EccChat), Schema = Schemas.EccSchema)]
    public class EccChat : IHaveIntId
    {
        public int Id { get; set; }

        public bool IsDialog { get; set; }

        public string ChatName { get; set; }

        public virtual ICollection<EccChatMessage> Messages { get; set; }

        public virtual ICollection<EccChatUserRelation> UserRelations { get; set; }
    }
}