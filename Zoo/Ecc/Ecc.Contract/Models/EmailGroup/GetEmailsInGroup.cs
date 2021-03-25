using Croco.Core.Contract.Models.Search;

namespace Ecc.Contract.Models.EmailGroup
{
    public class GetEmailsInGroup : GetListSearchModel
    {
        public string EmailGroupId { get; set; }
    }
}