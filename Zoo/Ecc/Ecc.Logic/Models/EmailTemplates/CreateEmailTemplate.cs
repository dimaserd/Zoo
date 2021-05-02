using System.ComponentModel.DataAnnotations;

namespace Ecc.Logic.Models.EmailTemplates
{
    /// <summary>
    /// Создать шаблон эмейла
    /// </summary>
    public class CreateEmailTemplate
    {
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
        /// Название типа шаблона
        /// </summary>
        public string CustomEmailType { get; set; }

        /// <summary>
        /// Данный джаваскрипт должен описывать две функции GetEmailBody() и GetEmailSubject
        /// </summary>
        [Display(Name = "Скрипт сообщения")]
        public string JsScript { get; set; }

        /// <summary>
        /// Флаг заскриптоанности
        /// </summary>
        public bool IsJsScripted { get; set; }
    }
}