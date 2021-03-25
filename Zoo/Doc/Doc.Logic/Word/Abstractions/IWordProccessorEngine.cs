using Croco.Core.Contract.Models;
using Doc.Logic.Word.Models;

namespace Doc.Logic.Word.Abstractions
{
    /// <summary>
    /// Процессор документов с раширеним docxs
    /// </summary>
    public interface IWordProccessorEngine
    {
        /// <summary>
        /// Создать документ на основе шаблона
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        BaseApiResponse ProccessTemplate(DocXDocumentObjectModel model);
    }
}