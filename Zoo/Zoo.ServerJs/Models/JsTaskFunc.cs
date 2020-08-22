using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zoo.ServerJs.Models.Method;

namespace Zoo.ServerJs.Models
{
    /// <summary>
    /// Обертка над таской в Javascript аналогичная <see cref="Task{TResult}"/>
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public class JsTaskFunc<TResult>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="task"></param>
        public JsTaskFunc(Func<Task<TResult>> task)
        {
            InnerTask = task;
        }

        Func<Task<TResult>> InnerTask { get; }

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
                        Result = InnerTask().GetAwaiter().GetResult(),
                    }
                }
            };
        }
    }

    /// <summary>
    /// Обертка над параметризированной таской в Javascript аналогичная вызову асинхронной функции с параметрами.
    /// <para></para>
    /// Результат вызова которой аналогичен <see cref="Task{TResult}"/>
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public class JsTaskFunc<T1, TResult>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="task"></param>
        public JsTaskFunc(Func<T1, Task<TResult>> task)
        {
            Task = task;
        }

        Func<T1, Task<TResult>> Task { get; }

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

    /// <summary>
    /// Обертка над параметризированной таской в Javascript аналогичная вызову асинхронной функции с параметрами.
    /// <para></para>
    /// Результат вызова которой аналогичен <see cref="Task{TResult}"/>
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public class JsTaskFunc<T1, T2, TResult>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="task"></param>
        public JsTaskFunc(Func<T1, T2, Task<TResult>> task)
        {
            Task = task;
        }

        Func<T1, T2, Task<TResult>> Task { get; }

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
                        Result = Task(p.GetParameter<T1>(), p.GetParameter<T2>()).GetAwaiter().GetResult(),
                    }
                }
            };
        }
    }
}