using System;
using System.Collections.Generic;

namespace Zoo.ServerJs.Models
{
    /// <summary>
    /// Обертка над функцией в Javascript аналогичная <see cref="Func{TResult}"/>
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public class JsFunc<TResult>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="func"></param>
        public JsFunc(Func<TResult> func)
        {
            Func = func;
        }

        Func<TResult> Func { get; }

        /// <summary>
        /// Получить документацию по методу
        /// </summary>
        /// <param name="opts"></param>
        /// <returns></returns>
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

    /// <summary>
    /// Обертка над функцией в Javascript аналогичная <see cref="Func{T, TResult}"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public class JsFunc<T, TResult>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="func"></param>
        public JsFunc(Func<T, TResult> func)
        {
            Func = func;
        }

        Func<T, TResult> Func { get; }

        /// <summary>
        /// Получить документацию по методу
        /// </summary>
        /// <param name="opts"></param>
        /// <returns></returns>
        public JsWorkerMethodDocs GetJsMethod(JsWorkerMethodDocsOptions opts)
        {
            return new JsWorkerMethodDocs
            {
                MethodName = opts.MethodName,
                Description = opts.Description,
                Parameters = new List<Type> { typeof(T) },
                Response = typeof(TResult),
                Method = new JsWorkerMethodBase
                {
                    FunctionLink = p => new JsWorkerMethodResult
                    {
                        Result = Func(p.GetParameter<T>()),
                    }
                }
            };
        }
    }

    /// <summary>
    /// Обертка над функцией в Javascript аналогичная <see cref="Func{T1, T2, TResult}"/>
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public class JsFunc<T1, T2, TResult>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="func"></param>
        public JsFunc(Func<T1, T2, TResult> func)
        {
            Func = func;
        }

        Func<T1, T2, TResult> Func { get; }

        /// <summary>
        /// Получить документацию по методу
        /// </summary>
        /// <param name="opts"></param>
        /// <returns></returns>
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

    /// <summary>
    /// Обертка над функцией в Javascript аналогичная <see cref="Func{T1, T2, T3, TResult}"/>
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public class JsFunc<T1, T2, T3, TResult>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="func"></param>
        public JsFunc(Func<T1, T2, T3, TResult> func)
        {
            Func = func;
        }

        Func<T1, T2, T3, TResult> Func { get; }

        /// <summary>
        /// Получить документацию по методу
        /// </summary>
        /// <param name="opts"></param>
        /// <returns></returns>
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