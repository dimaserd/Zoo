using System;
using System.Collections.Generic;

namespace Zoo.ServerJs.Models
{
    /// <summary>
    /// Обертка над методом в Javascript аналогичная <see cref="Action{T}"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class JsAction<T>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="action"></param>
        public JsAction(Action<T> action)
        {
            Action = action;
        }

        Action<T> Action { get; }

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
                Response = null,
                Method = new JsWorkerMethodBase
                {
                    FunctionLink = p => 
                    {
                        Action(p.GetParameter<T>());
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