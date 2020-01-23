using System;
using System.Collections.Generic;

namespace Zoo.ServerJs.Models
{
    public class JsFunc<TResult>
    {
        public JsFunc(Func<TResult> func)
        {
            Func = func;
        }

        Func<TResult> Func { get; }

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
                        Result = Func(),
                    }
                }
            };
        }
    }

    public class JsFunc<T1, TResult>
    {
        public JsFunc(Func<T1, TResult> func)
        {
            Func = func;
        }

        Func<T1, TResult> Func { get; }

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
                        Result = Func(p.GetParameter<T1>()),
                    }
                }
            };
        }
    }

    public class JsFunc<T1, T2, TResult>
    {
        public JsFunc(Func<T1, T2, TResult> func)
        {
            Func = func;
        }

        Func<T1, T2, TResult> Func { get; }

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
                        Result = Func(p.GetParameter<T1>(), p.GetParameter<T2>()),
                    }
                }
            };
        }
    }

    public class JsFunc<T1, T2, T3, TResult>
    {
        public JsFunc(Func<T1, T2, T3, TResult> func)
        {
            Func = func;
        }

        Func<T1, T2, T3, TResult> Func { get; }

        public JsWorkerMethodDocs GetJsMethod(JsWorkerMethodDocsOptions opts)
        {
            return new JsWorkerMethodDocs
            {
                MethodName = opts.MethodName,
                Description = opts.Description,
                Parameters = new List<Type> { typeof(T1), typeof(T2), typeof(T3) },
                Response = typeof(TResult),
                Method = new JsWorkerMethodBase
                {
                    FunctionLink = p => new JsWorkerMethodResult
                    {
                        Result = Func(p.GetParameter<T1>(), p.GetParameter<T2>(), p.GetParameter<T3>()),
                    }
                }
            };
        }
    }
}