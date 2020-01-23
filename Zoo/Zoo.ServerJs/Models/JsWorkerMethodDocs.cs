using System;
using System.Collections.Generic;

namespace Zoo.ServerJs.Models
{
    public class JsWorkerMethodDocs
    {
        /// <summary>
        /// Типы входных параметров
        /// </summary>
        public List<Type> Parameters { get; set; }

        /// <summary>
        /// Тип возвращаемого значения 
        /// </summary>
        public Type Response { get; set; }

        /// <summary>
        /// Ссылка на метод
        /// </summary>
        public JsWorkerMethodBase Method { get; set; }

        /// <summary>
        /// Название метода
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// Описание метода
        /// </summary>
        public string Description { get; set; }

        public static JsWorkerMethodDocs GetMethod<TResult>(JsWorkerMethodDocsOptions opts, JsTaskFunc<TResult> func)
        {
            return func.GetJsMethod(opts);
        }

        public static JsWorkerMethodDocs GetMethod<T1, TResult>(JsWorkerMethodDocsOptions opts, JsTaskFunc<T1, TResult> func)
        {
            return func.GetJsMethod(opts);
        }

        public static JsWorkerMethodDocs GetMethod<T1, T2, TResult>(JsWorkerMethodDocsOptions opts, JsTaskFunc<T1, T2, TResult> func)
        {
            return func.GetJsMethod(opts);
        }

        public static JsWorkerMethodDocs GetMethod<TResult>(JsWorkerMethodDocsOptions opts, JsFunc<TResult> func)
        {
            return func.GetJsMethod(opts);
        }

        public static JsWorkerMethodDocs GetMethod<T1, TResult>(JsWorkerMethodDocsOptions opts, JsFunc<T1, TResult> func)
        {
            return func.GetJsMethod(opts);
        }

        public static JsWorkerMethodDocs GetMethod<T1, T2, TResult>(JsWorkerMethodDocsOptions opts, JsFunc<T1, T2, TResult> func)
        {
            return func.GetJsMethod(opts);
        }

        public static JsWorkerMethodDocs GetMethod<T1, T2, T3, TResult>(JsWorkerMethodDocsOptions opts, JsFunc<T1, T2, T3, TResult> func)
        {
            return func.GetJsMethod(opts);
        }


        public static JsWorkerMethodDocs GetMethod<TParam>(JsWorkerMethodDocsOptions opts, JsAction<TParam> func)
        {
            return func.GetJsMethod(opts);
        }
    }
}