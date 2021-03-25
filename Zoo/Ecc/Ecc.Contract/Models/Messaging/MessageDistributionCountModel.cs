namespace Ecc.Contract.Models.Messaging
{
    public class MessageDistributionCountModel
    {
        public string Id { get; set; }

        public string Type { get; set; }

        public string Data { get; set; }

        public int InteractionsCount { get; set; }
    }
}