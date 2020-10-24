using Croco.Core.Documentation.Models;
using Croco.Core.Documentation.Services;
using Zoo.ServerJs.Abstractions;
using Zoo.ServerJs.Models.Method;
using Zoo.ServerJs.Services;

namespace Zoo.ServerJs.WorkerExamples
{
    /// <summary>
    /// Описыватель типов
    /// </summary>
    public class CrocoTypeDescriptionJsWorker : IJsWorker
    {
        string WorkerName { get; }

        public CrocoTypeDescriptionJsWorker(string workerName = "CrocoTypeDescription")
        {
            WorkerName = workerName;
        }

        /// <summary>
        /// Получить документацию по перечислению. 
        /// Если тип не найден, или найденный тип не является перечислением возвращает null.
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static CrocoEnumTypeDescription GetEnumTypeDocumentation(string typeName)
        {
            var typeDoc = GetTypeDocumentation(typeName).GetMainTypeDescription();

            if (typeDoc == null)
            {
                return null;
            }

            if (!typeDoc.IsEnumeration)
            {
                return null;
            }

            return typeDoc.EnumDescription;
        }

        /// <summary>
        /// Получить документацию по типу данных
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static CrocoTypeDescriptionResult GetTypeDocumentation(string typeName)
        {
            if (typeName == null)
            {
                return null;
            }

            var type = CrocoTypeSearcher.FindFirstTypeByName(typeName, x => !x.IsGenericTypeDefinition);

            if (type == null)
            {
                return null;
            }

            return CrocoTypeDescription.GetDescription(type);
        }

        public JsWorkerDocumentation JsWorkerDocs(JsWorkerBuilder builder)
        {
            return builder.SetWorkerName(WorkerName)
                .SetDescription("Описыватель типов")
                .AddMethodViaFunction<string, CrocoTypeDescriptionResult>(GetTypeDocumentation, new JsWorkerMethodDocsOptions
                {
                    MethodName = "Type",
                    Description = "Получить описание для типа",
                    ParameterDescriptions = new[] { "Полное или сокращенное название типа" },
                    ResultDescription = "Описание типа"
                })
                .AddMethodViaFunction<string, CrocoEnumTypeDescription>(GetEnumTypeDocumentation, new JsWorkerMethodDocsOptions
                {
                    MethodName = "Enum",
                    Description = "Получить описание для перечисления",
                    ParameterDescriptions = new[] { "Полное или сокращенное название типа (тип должен являться перечислением)" },
                    ResultDescription = "Описание перечисления"
                })
                .Build();
        }
    }
}