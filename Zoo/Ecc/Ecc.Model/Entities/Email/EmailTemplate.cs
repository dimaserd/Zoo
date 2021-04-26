using Croco.Core.Model.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecc.Model.Entities.Email
{
    /// <summary>
    /// Сущность описывающая Email шаблон
    /// </summary>
    [Table(nameof(EmailTemplate), Schema = Schemas.EccSchema)]
    public class EmailTemplate : AuditableEntityBase
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Тип шаблона сообщения
        /// </summary>
        [Display(Name = "Тип шаблона сообщения")]
        public string TemplateType { get; set; }

        /// <summary>
        /// Флаг активности
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Кастомный тип шаблона
        /// </summary>
        public string CustomEmailType { get; set; }

        /// <summary>
        /// Данный джаваскрипт должен описывать две функции GetEmailBody() и GetEmailSubject
        /// </summary>
        [Display(Name = "Скрипт сообщения")]
        public string JsScript { get; set; }

        /// <summary>
        /// Является ли шаблон скриптованным
        /// </summary>
        public bool IsJsScripted { get; set; }
    }
}