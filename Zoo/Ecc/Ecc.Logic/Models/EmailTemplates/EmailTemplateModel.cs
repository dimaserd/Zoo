using System.ComponentModel.DataAnnotations;

namespace Ecc.Logic.Models.EmailTemplates
{
    public class EmailTemplateModel
    {
        public string Id { get; set; }

        [Display(Name = "Тип сообщения")]
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