using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Zoo.ServerJs.Models
{
    public class JsTaskFunc<TResult>
    {
        public JsTaskFunc(Func<Task<TResult>> task)
        {
            Task = task;
        }

        Func<Task<TResult>> Task { get; }

        public JsWorkerMethodDocs GetJsMethod(JsWorkerMethodDocsOptions opts)
        {
            return new JsWorkerMethodDocs
            {
                MethodName = opts.MethodName,
                Description = opts.Description,
                Parameters = null,
                Response = typeof(TResult),
                Method = new JsWorkerMethodBase
                {
                    FunctionLink = p => new JsWorkerMethodResult
                    {
                        Result = Task().GetAwaiter().GetResult(),
                    }
                }
            };
        }
    }

    public class JsTaskFunc<T1, TResult>
    {
        public JsTaskFunc(Func<T1, Task<TResult>> task)
        {
            Task = task;
        }

        Func<T1, Task<TResult>> Task { get; }

        public JsWorkerMethodDocs GetJsMethod(JsWorkerMethodDocsOptions opts)
        {
            return new JsWorkerMethodDocs
            {
                MethodName = opts.MethodName,
                Description = opts.Description,
                Parameters = new List<Type> { typeof(T1) },
                Response = typeof(TResult),
                Method = new JsWorkerMethodBase
                {
                    FunctionLink = p => new JsWorkerMethodResult
                    {
                        Result = Task(p.GetParameter<T1>()).GetAwaiter().GetResult(),
                    }
                }
            };
        }
    }

    public class JsTaskFunc<T1, T2, TResult>
    {
        public JsTaskFunc(Func<T1, T2, Task<TResult>> task)
        {
            Task = task;
        }

        Func<T1, T2, Task<TResult>> Task { get; }

        public JsWorkerMethodDocs GetJsMethod(JsWorkerMethodDocsOptions opts)
        {
            return new JsWorkerMethodDocs
            {
                MethodName = opts.MethodName,
                Description = opts.Description,
                Parameters = new List<Type> { typeof(T1), typeof(T2) },
                Response = typeof(TResult),
                Method = new JsWorkerMethodBase
                {
                    FunctionLink = p => new JsWorkerMethodResult
                    {
                        Result = Task(p.GetParameter<T1>(), p.GetParameter<T2>()).GetAwaiter().GetResult(),
                    }
                }
            };
        }
    }
}