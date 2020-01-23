using System;
using System.Collections.Generic;

namespace Zoo.ServerJs.Models
{
    public class JsAction<T1>
    {
        public JsAction(Action<T1> action)
        {
            Action = action;
        }

        Action<T1> Action { get; }

        public JsWorkerMethodDocs GetJsMethod(JsWorkerMethodDocsOptions opts)
        {
            return new JsWorkerMethodDocs
            {
                MethodName = opts.MethodName,
                Description = opts.Description,
                Parameters = new List<Type> { typeof(T1) },
                Response = null,
                Method = new JsWorkerMethodBase
                {
                    FunctionLink = p => 
                    {
                        Action(p.GetParameter<T1>());
                        return new JsWorkerMethodResult
                        {
                            Result = null,
                        };
                    }
                }
            };
        }
    }
}