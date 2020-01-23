using Croco.Core.Documentation.Models;
using System.Collections.Generic;
using System.Linq;

namespace Zoo.ServerJs.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class JsOpenApiWorkerMethodDocumentation
    {
        public JsOpenApiWorkerMethodDocumentation(JsWorkerMethodDocs methodDocs)
        {
            MethodName = methodDocs.MethodName;
            Description = methodDocs.Description;

            if(methodDocs.Response != null)
            {
                Response = CrocoTypeDescription.GetDescription(methodDocs.Response);
            }
            
            if(methodDocs.Parameters != null)
            {
                Parameters = methodDocs.Parameters.Select(x => CrocoTypeDescription.GetDescription(x)).ToList();
            }
        }

        public string MethodName { get; set; }

        public string Description { get; set; }

        public CrocoTypeDescription Response { get; set; }

        public List<CrocoTypeDescription> Parameters { get; set; }
    }
}