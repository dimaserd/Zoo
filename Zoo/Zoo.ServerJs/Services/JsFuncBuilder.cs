using Croco.Core.Documentation.Models.Methods;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zoo.ServerJs.Models.Method;

namespace Zoo.ServerJs.Services
{
    internal static class JsFuncBuilder
    {
        internal static JsWorkerMethodDocs GetFunc<TResult>(Func<TResult> func, MethodDocsOptions opts)
        {
            return new JsWorkerMethodDocs
            {
                Description = MethodDescription.Create(typeof(TResult), null, opts),
                Method = new JsWorkerMethodBase
                {
                    FunctionLink = (p, srv) =>
                    {
                        var result = func();

                        return new JsWorkerMethodResult
                        {
                            Result = result
                        };
                    }
                }
            };
        }

        internal static JsWorkerMethodDocs GetFunc<T, TResult>(Func<T, TResult> func, MethodDocsOptions opts)
        {
            return new JsWorkerMethodDocs
            {
                Description = MethodDescription.Create(typeof(TResult), new List<Type> { typeof(T) }, opts),
                Method = new JsWorkerMethodBase
                {
                    FunctionLink = (p, srv) =>
                    {
                        var result = func(p.GetParameter<T>());

                        return new JsWorkerMethodResult
                        {
                            Result = result
                        };
                    }
                }
            };
        }

        internal static JsWorkerMethodDocs GetFunc<T1, T2, TResult>(Func<T1, T2, TResult> func, MethodDocsOptions opts)
        {
            return new JsWorkerMethodDocs
            {
                Description = MethodDescription.Create(typeof(TResult), new List<Type> { typeof(T1), typeof(T2) }, opts),
                Method = new JsWorkerMethodBase
                {
                    FunctionLink = (p, srv) =>
                    {
                        var result = func(p.GetParameter<T1>(), p.GetParameter<T2>());

                        return new JsWorkerMethodResult
                        {
                            Result = result
                        };
                    }
                }
            };
        }

        internal static JsWorkerMethodDocs GetFunc<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> func, MethodDocsOptions opts)
        {
            return new JsWorkerMethodDocs
            {
                Description = MethodDescription.Create(typeof(TResult), new List<Type> { typeof(T1), typeof(T2), typeof(T3) }, opts),
                Method = new JsWorkerMethodBase
                {
                    FunctionLink = (p, srv) =>
                    {
                        var result = func(p.GetParameter<T1>(), p.GetParameter<T2>(), p.GetParameter<T3>());

                        return new JsWorkerMethodResult
                        {
                            Result = result
                        };
                    }
                }
            };
        }

        internal static JsWorkerMethodDocs GetAction(Action action, MethodDocsOptions opts)
        {
            return new JsWorkerMethodDocs
            {
                Description = MethodDescription.Create(null, null, opts),
                Method = new JsWorkerMethodBase
                {
                    FunctionLink = (p, srv) =>
                    {
                        action();
                        return new JsWorkerMethodResult
                        {
                            Result = null,
                        };
                    }
                }
            };
        }

        internal static JsWorkerMethodDocs GetAction<T1>(Action<T1> action, MethodDocsOptions opts)
        {
            return new JsWorkerMethodDocs
            {
                Description = MethodDescription.Create(null, new List<Type> { typeof(T1) }, opts),
                Method = new JsWorkerMethodBase
                {
                    FunctionLink = (p, srv) =>
                    {
                        action(p.GetParameter<T1>());
                        return new JsWorkerMethodResult
                        {
                            Result = null,
                        };
                    }
                }
            };
        }

        internal static JsWorkerMethodDocs GetAction<T1, T2>(Action<T1, T2> action, MethodDocsOptions opts)
        {
            return new JsWorkerMethodDocs
            {
                Description = MethodDescription.Create(null, new List<Type> { typeof(T1), typeof(T2) }, opts),
                Method = new JsWorkerMethodBase
                {
                    FunctionLink = (p, srv) =>
                    {
                        action(p.GetParameter<T1>(), p.GetParameter<T2>());
                        return new JsWorkerMethodResult
                        {
                            Result = null,
                        };
                    }
                }
            };
        }

        internal static JsWorkerMethodDocs GetTask(Func<Task> task, MethodDocsOptions opts)
        {
            return new JsWorkerMethodDocs
            {
                Description = MethodDescription.Create(null, null, opts),
                Method = new JsWorkerMethodBase
                {
                    FunctionLink = (p, srv) =>
                    {
                        task().ConfigureAwait(true).GetAwaiter().GetResult();
                        return new JsWorkerMethodResult
                        {
                            Result = null,
                        };
                    }
                }
            };
        }

        internal static JsWorkerMethodDocs GetTask<T1>(Func<T1, Task> task, MethodDocsOptions opts)
        {
            return new JsWorkerMethodDocs
            {
                Description = MethodDescription.Create(null, new List<Type> { typeof(T1) }, opts),
                Method = new JsWorkerMethodBase
                {
                    FunctionLink = (p, srv) =>
                    {
                        task(p.GetParameter<T1>()).ConfigureAwait(true).GetAwaiter().GetResult();
                        return new JsWorkerMethodResult
                        {
                            Result = null,
                        };
                    }
                }
            };
        }

        internal static JsWorkerMethodDocs GetTask<TResult>(Func<Task<TResult>> task, MethodDocsOptions opts)
        {
            return new JsWorkerMethodDocs
            {
                Description = MethodDescription.Create(typeof(TResult), null, opts),
                Method = new JsWorkerMethodBase
                {
                    FunctionLink = (p, srv) =>
                    {
                        return new JsWorkerMethodResult
                        {
                            Result = task().ConfigureAwait(true).GetAwaiter().GetResult(),
                        };
                    }
                }
            };
        }

        internal static JsWorkerMethodDocs GetTask<T1, TResult>(Func<T1, Task<TResult>> task, MethodDocsOptions opts)
        {
            return new JsWorkerMethodDocs
            {
                Description = MethodDescription.Create(typeof(TResult), new List<Type> { typeof(T1) }, opts),
                Method = new JsWorkerMethodBase
                {
                    FunctionLink = (p, srv) =>
                    {
                        return new JsWorkerMethodResult
                        {
                            Result = task(p.GetParameter<T1>()).ConfigureAwait(true).GetAwaiter().GetResult(),
                        };
                    }
                }
            };
        }

        internal static JsWorkerMethodDocs GetTask<T1, T2, TResult>(Func<T1, T2, Task<TResult>> task, MethodDocsOptions opts)
        {
            return new JsWorkerMethodDocs
            {
                Description = MethodDescription.Create(typeof(TResult), new List<Type> { typeof(T1), typeof(T2) }, opts),
                Method = new JsWorkerMethodBase
                {
                    FunctionLink = (p, srv) =>
                    {
                        return new JsWorkerMethodResult
                        {
                            Result = task(p.GetParameter<T1>(), p.GetParameter<T2>()).ConfigureAwait(true).GetAwaiter().GetResult(),
                        };
                    }
                }
            };
        }
    }
}