using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zoo.ServerJs.Models.Method;

namespace Zoo.ServerJs.Services
{
    internal static class JsServiceFuncBuilder
    {
        internal static JsWorkerMethodDocs GetFunc<TService, TResult>(Func<TService, TResult> func, JsWorkerMethodDocsOptions opts)
        {
            return new JsWorkerMethodDocs(typeof(TResult), null, opts)
            {
                Method = new JsWorkerMethodBase
                {
                    FunctionLink = (p, srv) =>
                    {
                        var service = srv.GetRequiredService<TService>();

                        var result = func(service);

                        return new JsWorkerMethodResult
                        {
                            Result = result
                        };
                    }
                }
            };
        }

        internal static JsWorkerMethodDocs GetFunc<TService, T, TResult>(Func<TService, T, TResult> func, JsWorkerMethodDocsOptions opts)
        {
            return new JsWorkerMethodDocs(typeof(TResult), new List<Type> { typeof(T) }, opts)
            {
                Method = new JsWorkerMethodBase
                {
                    FunctionLink = (p, srv) =>
                    {
                        var service = srv.GetRequiredService<TService>();

                        var result = func(service, p.GetParameter<T>());

                        return new JsWorkerMethodResult
                        {
                            Result = result
                        };
                    }
                }
            };
        }

        internal static JsWorkerMethodDocs GetFunc<TService, T1, T2, TResult>(Func<TService, T1, T2, TResult> func, JsWorkerMethodDocsOptions opts)
        {
            return new JsWorkerMethodDocs(typeof(TResult), new List<Type> { typeof(T1), typeof(T2) }, opts)
            {
                Method = new JsWorkerMethodBase
                {
                    FunctionLink = (p, srv) =>
                    {
                        var service = srv.GetRequiredService<TService>();

                        var result = func(service, p.GetParameter<T1>(), p.GetParameter<T2>());

                        return new JsWorkerMethodResult
                        {
                            Result = result
                        };
                    }
                }
            };
        }

        internal static JsWorkerMethodDocs GetFunc<TService, T1, T2, T3, TResult>(Func<TService, T1, T2, T3, TResult> func, JsWorkerMethodDocsOptions opts)
        {
            return new JsWorkerMethodDocs(typeof(TResult), new List<Type> { typeof(T1), typeof(T2), typeof(T3) }, opts)
            {
                Method = new JsWorkerMethodBase
                {
                    FunctionLink = (p, srv) =>
                    {
                        var service = srv.GetRequiredService<TService>();

                        var result = func(service, p.GetParameter<T1>(), p.GetParameter<T2>(), p.GetParameter<T3>());

                        return new JsWorkerMethodResult
                        {
                            Result = result
                        };
                    }
                }
            };
        }

        internal static JsWorkerMethodDocs GetAction<TService>(Action<TService> action, JsWorkerMethodDocsOptions opts)
        {
            return new JsWorkerMethodDocs(null, null, opts)
            {
                Method = new JsWorkerMethodBase
                {
                    FunctionLink = (p, srv) =>
                    {
                        var service = srv.GetRequiredService<TService>();
                        action(service);
                        return new JsWorkerMethodResult
                        {
                            Result = null,
                        };
                    }
                }
            };
        }

        internal static JsWorkerMethodDocs GetAction<TService, T1>(Action<TService, T1> action, JsWorkerMethodDocsOptions opts)
        {
            return new JsWorkerMethodDocs(null, new List<Type> { typeof(T1) }, opts)
            {
                Method = new JsWorkerMethodBase
                {
                    FunctionLink = (p, srv) =>
                    {
                        var service = srv.GetRequiredService<TService>();

                        action(service, p.GetParameter<T1>());
                        return new JsWorkerMethodResult
                        {
                            Result = null,
                        };
                    }
                }
            };
        }

        internal static JsWorkerMethodDocs GetAction<TService, T1, T2>(Action<TService, T1, T2> action, JsWorkerMethodDocsOptions opts)
        {
            return new JsWorkerMethodDocs(null, new List<Type> { typeof(T1), typeof(T2) }, opts)
            {
                Method = new JsWorkerMethodBase
                {
                    FunctionLink = (p, srv) =>
                    {
                        var service = srv.GetRequiredService<TService>();
                        action(service, p.GetParameter<T1>(), p.GetParameter<T2>());
                        return new JsWorkerMethodResult
                        {
                            Result = null,
                        };
                    }
                }
            };
        }

        internal static JsWorkerMethodDocs GetTask<TService>(Func<TService, Task> task, JsWorkerMethodDocsOptions opts)
        {
            return new JsWorkerMethodDocs(null, null, opts)
            {
                Method = new JsWorkerMethodBase
                {
                    FunctionLink = (p, srv) =>
                    {
                        var service = srv.GetRequiredService<TService>();
                        task(service).ConfigureAwait(true).GetAwaiter().GetResult();
                        return new JsWorkerMethodResult
                        {
                            Result = null,
                        };
                    }
                }
            };
        }

        internal static JsWorkerMethodDocs GetTask<TService, TResult>(Func<TService, Task<TResult>> task, JsWorkerMethodDocsOptions opts)
        {
            return new JsWorkerMethodDocs(typeof(TResult), null, opts)
            {
                Method = new JsWorkerMethodBase
                {
                    FunctionLink = (p, srv) =>
                    {
                        var service = srv.GetRequiredService<TService>();

                        return new JsWorkerMethodResult
                        {
                            Result = task(service).ConfigureAwait(true).GetAwaiter().GetResult(),
                        };
                    }
                }
            };
        }

        internal static JsWorkerMethodDocs GetTask<TService, T1, TResult>(Func<TService, T1, Task<TResult>> task, JsWorkerMethodDocsOptions opts)
        {
            return new JsWorkerMethodDocs(typeof(TResult), new List<Type> { typeof(T1) }, opts)
            {
                Method = new JsWorkerMethodBase
                {
                    FunctionLink = (p, srv) =>
                    {
                        var service = srv.GetRequiredService<TService>();

                        return new JsWorkerMethodResult
                        {
                            Result = task(service, p.GetParameter<T1>()).ConfigureAwait(true).GetAwaiter().GetResult(),
                        };
                    }
                }
            };
        }

        internal static JsWorkerMethodDocs GetTask<TService, T1, T2, TResult>(Func<TService, T1, T2, Task<TResult>> task, JsWorkerMethodDocsOptions opts)
        {
            return new JsWorkerMethodDocs(typeof(TResult), new List<Type> { typeof(T1), typeof(T2) }, opts)
            {
                Method = new JsWorkerMethodBase
                {
                    FunctionLink = (p, srv) =>
                    {
                        var service = srv.GetRequiredService<TService>();

                        return new JsWorkerMethodResult
                        {
                            Result = task(service, p.GetParameter<T1>(), p.GetParameter<T2>()).ConfigureAwait(true).GetAwaiter().GetResult(),
                        };
                    }
                }
            };
        }
    }
}