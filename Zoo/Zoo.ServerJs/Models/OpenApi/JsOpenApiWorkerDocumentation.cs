using System.Collections.Generic;
using System.Linq;
using Zoo.ServerJs.Models.Method;

namespace Zoo.ServerJs.Models.OpenApi
{
    /// <summary>
    /// Описание серверного javascript обработчика
    /// </summary>
    public class JsOpenApiWorkerDocumentation
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="docs"></param>
        internal JsOpenApiWorkerDocumentation(JsWorkerDocumentation docs)
        {
            WorkerName = docs.WorkerName;
            Description = docs.Description;
            Methods = docs.Methods.Select(x => new JsOpenApiWorkerMethodDocumentation(x.Value)).ToList();
        }

        /// <summary>
        /// Дефолтный конструктор
        /// </summary>
        public JsOpenApiWorkerDocumentation()
        {
        }

        /// <summary>
        /// Название обработчика
        /// </summary>
        public string WorkerName { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Методы
        /// </summary>
        public List<JsOpenApiWorkerMethodDocumentation> Methods { get; set; }
    }
}