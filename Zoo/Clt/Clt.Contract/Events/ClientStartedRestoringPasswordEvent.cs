namespace Clt.Contract.Events
{
    public class ClientStartedRestoringPasswordEvent
    {
        public string Code { get; set; }

        public string UserId { get; set; }
    }
}