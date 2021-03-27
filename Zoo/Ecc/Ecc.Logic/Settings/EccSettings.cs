namespace Ecc.Logic.Settings
{
    public class EccSettings
    {
        public string ApplicationUrl { get; set; }
        public EccLinkFunctionInvokerSettings FunctionInvokerSettings { get; set; }
        public PixelUrlProviderOptions PixelUrlProviderOptions { get; set; }
    }
}