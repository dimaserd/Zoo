using System.ComponentModel.DataAnnotations;

namespace Ecc.Logic.Models.EmailTemplates
{
    /// <summary>
    /// Модель описывающая шаблон эмейлв
    /// </summary>
    public class EmailTemplateModel
    {
        /// <summary>
        /// Идентифиатор
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Тип сообощения
        /// </summary>
        [Display(Name = "Тип сообщения")]
        public string TemplateType { get; set; }

        /// <summary>
        /// Флаг активности
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Тип шаблона
        /// </summary>
        public string CustomEmailType { get; set; }

        /// <summary>
        /// Данный джаваскрипт должен описывать две функции GetEmailBody() и GetEmailSubject
        /// </summary>
        [Display(Name = "Скрипт сообщения")]
        public string JsScript { get; set; }

        /// <summary>
        /// Является ли письмо заскриптованным
        /// </summary>
        public bool IsJsScripted { get; set; }
    }
}