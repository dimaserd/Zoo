using System;
using System.Collections.Generic;
using System.Linq;

namespace Zoo.ServerJs.Models
{
    public class JsWorkerDocumentation
    {
        public string WorkerName { get; set; }

        public string Description { get; set; }

        public List<JsWorkerMethodDocs> Methods { get; set; }

        public JsOpenApiWorkerDocumentation GetOpenApiDocumentation()
        {
            return new JsOpenApiWorkerDocumentation(this);
        }

        public JsWorkerMethodResult HandleCall(string methodName, JsWorkerMethodCallParameters parameters)
        {
            var method = Methods.FirstOrDefault(x => x.MethodName == methodName);

            if(method == null)
            {
                throw new ApplicationException($"Метод с названием {methodName} не существует в группе методов класса {WorkerName}");
            }

            return method.Method.HandleCall(parameters);
        }
    }
}