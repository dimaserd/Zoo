namespace Ecc.Logic.Settings
{
    /// <summary>
    /// Основные настройки пконтекста рассылок
    /// </summary>
    public class EccSettings
    {
        /// <summary>
        /// Адрес приложения
        /// </summary>
        public string ApplicationUrl { get; set; }

        /// <summary>
        /// Настройки для редиректа ссылок в письмах
        /// </summary>
        public EccLinkFunctionInvokerSettings FunctionInvokerSettings { get; set; }

        /// <summary>
        /// Опции для создания провайдера пикселя
        /// </summary>
        public PixelUrlProviderOptions PixelUrlProviderOptions { get; set; }
    }
}