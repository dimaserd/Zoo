namespace Clt.Contract.Settings
{
    public class RightsSettings
    {
        public RightsSettings()
        {
            RootEmail = "root@mail.com";
            RootPassword = "RootPassword@1234";
            UserRemovingEnabled = true;
        }

        public string RootEmail { get; set; }

        public string RootPassword { get; set; }

        public bool UserRemovingEnabled { get; set; }
    }
}