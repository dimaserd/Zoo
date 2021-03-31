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
        public EccLinkFunctionInvokerSettings FunctionInvokerSettings { get; set; }
        public PixelUrlProviderOptions PixelUrlProviderOptions { get; set; }
    }
}