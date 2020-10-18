namespace Zoo.ServerJs.Models.OpenApi
{
    internal class RemoteApiResponseRecord
    {
        public string HostName { get; set; }
        public string HostUrl { get; set; }
        public string Request { get; set; }
        public string RequestUrl { get; set; }
        public int ResponseStatusCode { get; set; }
        public string Response { get; set; }
    }
}