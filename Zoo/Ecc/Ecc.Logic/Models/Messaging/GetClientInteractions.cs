using Croco.Core.Contract.Models.Search;
using Croco.Core.Search.Extensions;
using Ecc.Model.Entities.Interactions;
using System.Collections.Generic;

namespace Ecc.Logic.Models.Messaging
{
    public class GetClientInteractions : GetListSearchModel
    {
        public string ClientId { get; set; }

        public IEnumerable<SearchQueryCriteria<MailMessageInteraction>> GetCriterias()
        {
            yield return ClientId.MapString(x => new SearchQueryCriteria<MailMessageInteraction>(t => t.UserId == x));
        }
    }
}