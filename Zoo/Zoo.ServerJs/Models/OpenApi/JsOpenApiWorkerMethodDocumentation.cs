using Croco.Core.Documentation.Models;
using System.Collections.Generic;
using System.Linq;
using Zoo.ServerJs.Models.Method;

namespace Zoo.ServerJs.Models.OpenApi
{
    /// <summary>
    /// 
    /// </summary>
    public class JsOpenApiWorkerMethodDocumentation
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="methodDocs"></param>
        public JsOpenApiWorkerMethodDocumentation(JsWorkerMethodDocs methodDocs)
        {
            MethodName = methodDocs.MethodName;
            Description = methodDocs.Description;
            ParameterDescriptions = methodDocs.ParameterDescriptions;
            ResultDescription = methodDocs.ResultDescription;

            if (methodDocs.Response != null)
            {
                Response = CrocoTypeDescription.GetDescription(methodDocs.Response);
            }

            if (methodDocs.Parameters != null)
            {
                Parameters = methodDocs.Parameters.Select(x => CrocoTypeDescription.GetDescription(x)).ToList();
            }
        }

        /// <summary>
        /// Название метода
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Описание результата
        /// </summary>
        public string ResultDescription { get; set; }

        /// <summary>
        /// Описание параметров
        /// </summary>
        public string[] ParameterDescriptions { get; set; }

        /// <summary>
        /// Ответ от метода обработчика. Подробное описание типа.
        /// </summary>
        public CrocoTypeDescriptionResult Response { get; set; }

        /// <summary>
        /// Параметры для метода обработчика. Подробное описание типа.
        /// </summary>
        public List<CrocoTypeDescriptionResult> Parameters { get; set; }
    }
}