using Croco.Core.Contract.Models.Search;
using Croco.Core.Search.Extensions;
using Ecc.Model.Entities.Interactions;
using System.Collections.Generic;

namespace Ecc.Logic.Models.Messaging
{
    /// <summary>
    /// Получить взаимодействия с клиентом
    /// </summary>
    public class GetClientInteractions : GetListSearchModel
    {
        /// <summary>
        /// Идентификатор клиента
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Получить критерии поиска
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SearchQueryCriteria<MailMessageInteraction>> GetCriterias()
        {
            yield return ClientId.MapString(x => new SearchQueryCriteria<MailMessageInteraction>(t => t.UserId == x));
        }
    }
}