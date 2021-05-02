namespace Ecc.Contract.Models.EmailRedirects
{
    /// <summary>
    /// Модель описывающая пойманый эмейл в письме
    /// </summary>
    public class EmailLinkCatchRedirectsCountModel
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Пойманный урл
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Количество пойманных перенаправлений
        /// </summary>
        public int Count { get; set; }
    }
}