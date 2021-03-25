using Croco.Core.Model.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecc.Model.Entities.Email
{
    [Table(nameof(EmailTemplate), Schema = Schemas.EccSchema)]
    public class EmailTemplate : AuditableEntityBase
    {
        public string Id { get; set; }

        [Display(Name = "Тип шаблона сообiщения")]
        public string TemplateType { get; set; }

        public bool IsActive { get; set; }

        public string CustomEmailType { get; set; }

        /// <summary>
        /// Данный джаваскрипт должен описывать две функции GetEmailBody() и GetEmailSubject
        /// </summary>
        [Display(Name = "Скрипт сообщения")]
        public string JsScript { get; set; }

        public bool IsJsScripted { get; set; }
    }
}