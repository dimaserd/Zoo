using System;

namespace Zoo.ServerJs.Models
{
    public class JsWorkerMethodBase
    {
        public Func<JsWorkerMethodCallParameters, JsWorkerMethodResult> FunctionLink { get; set; }

        public JsWorkerMethodResult HandleCall(JsWorkerMethodCallParameters parameters)
        {
            return FunctionLink(parameters);
        }
    }
}