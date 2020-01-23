using System.Collections.Generic;
using System.Linq;

namespace Zoo.ServerJs.Models
{
    public class JsOpenApiWorkerDocumentation
    {
        public JsOpenApiWorkerDocumentation(JsWorkerDocumentation docs)
        {
            WorkerName = docs.WorkerName;
            Description = docs.Description;
            Methods = docs.Methods.Select(x => new JsOpenApiWorkerMethodDocumentation(x)).ToList();
        }

        public string WorkerName { get; set; }

        public string Description { get; set; }

        public List<JsOpenApiWorkerMethodDocumentation> Methods { get; set; }
    }
}