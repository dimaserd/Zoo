namespace Clt.Contract.Events
{
    public class ClientRegisteredEvent
    {
        public string UserId { get; set; }

        public bool IsPasswordGenerated { get; set; }

        public string Password { get; set; }
    }
}