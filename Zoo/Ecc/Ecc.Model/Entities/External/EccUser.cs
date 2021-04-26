using System.ComponentModel.DataAnnotations.Schema;
using Croco.Core.Contract.Data.Entities.HaveId;

namespace Ecc.Model.Entities.External
{
    /// <summary>
    /// Сущность описывающая пользователя
    /// </summary>
    [Table(nameof(EccUser), Schema = Schemas.EccSchema)]
    public class EccUser : IHaveStringId
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public string Id { get; set; }

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