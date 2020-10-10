using System;

namespace Zoo.RemoteApp
{
    public class CallRemoteProcedure
    {
        public string ClientId { get; set; }
        public string AuthToken { get; set; }
        public MethodPayload Payload { get; set; }
    }

    public class MethodPayload
    {
        public string MethodGroup { get; set; }
        public string Method { get; set; }
        public string DataJson { get; set; }
    }
}
