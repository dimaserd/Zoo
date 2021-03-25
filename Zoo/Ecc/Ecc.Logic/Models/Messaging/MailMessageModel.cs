using Ecc.Model.Entities.Interactions;
using Ecc.Common.Enumerations;
using System;
using System.Linq.Expressions;

namespace Ecc.Logic.Models.Messaging
{
    public class MailMessageModel
    {
        public string Id { get; set; }

        public string Body { get; set; }

        public string Header { get; set; }

        public DateTime? ReadOn { get; set; }

        public DateTime? SentOn { get; set; }

        public string EmailAddress { get; set; }

        public InteractionStatus Status { get; set; }

        public static Expression<Func<ApplicationInteractionWithStatus<MailMessageInteraction>, MailMessageModel>> SelectExpression = x => new MailMessageModel
        {
            Id = x.Interaction.Id,
            Body = x.Interaction.MessageText,
            Header = x.Interaction.TitleText,
            ReadOn = x.Interaction.ReadOn,
            SentOn = x.Interaction.SentOn,
            EmailAddress = x.Interaction.ReceiverEmail,
            Status = x.Status
        };
    }
}