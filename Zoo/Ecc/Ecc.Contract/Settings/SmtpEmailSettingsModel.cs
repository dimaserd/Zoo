namespace Ecc.Contract.Settings
{
    public class SmtpEmailSettingsModel
    {
        public string FromAddress { get; set; }
        public bool IsBodyHtml { get; set; }
        public string SmtpClientString { get; set; }
        public int SmtpPort { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}