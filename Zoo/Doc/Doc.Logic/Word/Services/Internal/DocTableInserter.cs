using Doc.Logic.Word.Implementations;
using Doc.Logic.Word.Models;
using DocumentFormat.OpenXml.Packaging;
using System;
using System.Linq;

namespace Doc.Logic.Word.Services.Internal
{
    internal static class DocTableInserter
    {
        public static void AddTable(WordprocessingDocument doc, DocumentTable tableModel)
        {
            var body = doc.MainDocumentPart
                    .Document.Body;

            var elem = body.ChildElements.FirstOrDefault(el => el.InnerText == tableModel.PlacingText);

            if (elem == null)
            {
                throw new ApplicationException($"Не найден элемент с внутренним текстом '{tableModel.PlacingText}', " +
                    $"вместо которого нужно вставить таблицу.");
            }

            var table = DocTableCreator.GetTable(tableModel);

            elem.InsertAfterSelf(table);

            elem.Remove();
        }
    }
}