using Croco.Core.Contract.Models.Search;

namespace Ecc.Contract.Models.EmailGroup
{
    /// <summary>
    /// Получить эмейлы в группе
    /// </summary>
    public class GetEmailsInGroup : GetListSearchModel
    {
        /// <summary>
        /// Идентификатор группы
        /// </summary>
        public string EmailGroupId { get; set; }
    }
}