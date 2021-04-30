using System;
using System.Collections.Generic;

namespace Ecc.Contract.Models.EmailRedirects
{
    /// <summary>
    /// Модель описывающая пойманный урл в эмейле
    /// </summary>
    public class EmailLinkCatchDetailedModel
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Урл
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Редиректы
        /// </summary>
        public List<DateTime> RedirectsOn { get; set; }
    }
}