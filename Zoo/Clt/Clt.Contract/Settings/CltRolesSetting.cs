namespace Clt.Contract.Settings
{
    public class CltRolesSetting
    {
        public string AdminRoleName { get; set; }

        public string RootRoleName { get; set; }

        public CltRolesSetting()
        {
            AdminRoleName = "Admin";
            RootRoleName = "Root";
        }
    }
}