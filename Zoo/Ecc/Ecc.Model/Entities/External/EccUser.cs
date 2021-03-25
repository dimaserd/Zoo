using System.ComponentModel.DataAnnotations.Schema;
using Croco.Core.Contract.Data.Entities.HaveId;

namespace Ecc.Model.Entities.External
{
    [Table(nameof(EccUser), Schema = Schemas.EccSchema)]
    public class EccUser : IHaveStringId
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }
    }
}