using Ecc.Contract.Models.EmailRedirects;
using Ecc.Model.Entities.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Ecc.Logic.Models.Messaging
{
    public class MailMessageDetailedModel
    {
        public string Id { get; set; }

        public string Body { get; set; }

        public string Header { get; set; }

        public DateTime? ReadOn { get; set; }

        public DateTime? SentOn { get; set; }

        public string EmailAddress { get; set; }

        public List<InteractionStatusModel> Statuses { get; set; }

        public List<EmailLinkCatchRedirectsCountModel> Redirects { get; set; }

        public static Expression<Func<MailMessageInteraction, MailMessageDetailedModel>> SelectExpression = x => new MailMessageDetailedModel
        {
            Id = x.Id,
            Body = x.MessageText,
            Header = x.TitleText,
            ReadOn = x.ReadOn,
            SentOn = x.SentOn,
            EmailAddress = x.ReceiverEmail,
            Statuses = x.Statuses.OrderByDescending(t => t.StartedOn).Select(t => new InteractionStatusModel
            {
                StartedOn = t.StartedOn,
                Status = t.Status,
                StatusDescription = t.StatusDescription
            }).ToList()
        };
    }
}