using System.Collections.Generic;

namespace Zoo.ServerJs.Models.OpenApi
{
    /// <summary>
    /// Описание открытого Js API
    /// </summary>
    public class JsOpenApiDocs
    {
        /// <summary>
        /// Рабочие классы интегрированные с c# логикой
        /// </summary>
        public List<JsOpenApiWorkerDocumentation> Workers { get; set; }

        /// <summary>
        /// Внешние Js компоненты
        /// </summary>
        public List<ExternalJsComponent> ExternalJsComponents { get; set; }
    }
}