using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Croco.Core.Contract.Data.Entities.HaveId;
using Ecc.Model.Entities.External;

namespace Ecc.Model.Entities.IntegratedApps
{
    /// <summary>
    /// Модель пользователя интегрированного с внешним приложением
    /// </summary>
    [Table(nameof(IntegratedAppUserSetting), Schema = Schemas.EccSchema)]
    public class IntegratedAppUserSetting : IHaveStringId
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        /// <summary>
        /// Пользователь
        /// </summary>
        public virtual EccUser User { get; set; }

        /// <summary>
        /// Идентифкаор приложения
        /// </summary>
        [ForeignKey(nameof(App))]
        public int AppId { get; set; }

        /// <summary>
        /// Приложение
        /// </summary>
        public virtual IntegratedApp App { get; set; }

        public bool Active { get; set; }

        [Required]
        public string UserUidInApp { get; set; }
    }
}