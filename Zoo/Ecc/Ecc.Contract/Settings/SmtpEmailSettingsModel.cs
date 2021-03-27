namespace Ecc.Contract.Settings
{
    /// <summary>
    /// Настройки для Smtp клиента
    /// </summary>
    public class SmtpEmailSettingsModel
    {
        /// <summary>
        /// С какого адреса будет отправлено сообщение
        /// </summary>
        public string FromAddress { get; set; }

        /// <summary>
        /// Является ли тело сообщения Html разметкой
        /// </summary>
        public bool IsBodyHtml { get; set; }

        /// <summary>
        /// Строка подключения к smtp
        /// </summary>
        public string SmtpClientString { get; set; }

        /// <summary>
        /// Порт
        /// </summary>
        public int SmtpPort { get; set; }
        
        /// <summary>
        /// Логин
        /// </summary>
        public string UserName { get; set; }
        
        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }
    }
}