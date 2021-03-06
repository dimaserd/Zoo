﻿using Croco.Core.Documentation.Models.Methods;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using Zoo.ServerJs.Models.Method;
using Zoo.ServerJs.Resources;

namespace Zoo.ServerJs.Services
{
    /// <summary>
    /// Построитель методов для Js рабочего
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    public class JsWorkerServiceMethodBuilder<TService>
    {
        JsWorkerBuilder Builder { get; }

        static readonly Type[] ExceptionalTypes = new Type[]
        {
            typeof(JsExecutor)
        };

        internal JsWorkerServiceMethodBuilder(JsWorkerBuilder builder, IServiceCollection serviceCollection)
        {
            Builder = builder;

            var serviceType = typeof(TService);
            
            if(!serviceCollection.Any(x => x.ServiceType == serviceType) && !ExceptionalTypes.Contains(serviceType))
            {
                throw new InvalidOperationException(string.Format(ExceptionTexts.TypeOfServiceNotRegisteredInServiceCollectionFormat, serviceType.FullName));
            }
        }
        
        /// <summary>
        /// Добавить метод
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <param name="opts"></param>
        /// <returns></returns>
        public JsWorkerServiceMethodBuilder<TService> AddMethodViaFunction<TResult>(Func<TService, TResult> func, MethodDocsOptions opts)
        {
            return AddMethod(opts, () => JsServiceFuncBuilder.GetFunc(func, opts));
        }

        /// <summary>
        /// Добавить метод
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <param name="opts"></param>
        /// <returns></returns>
        public JsWorkerServiceMethodBuilder<TService> AddMethodViaFunction<T1, TResult>(Func<TService, T1, TResult> func, MethodDocsOptions opts)
        {
            return AddMethod(opts, () => JsServiceFuncBuilder.GetFunc(func, opts));
        }

        /// <summary>
        /// Добавить метод
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <param name="opts"></param>
        /// <returns></returns>
        public JsWorkerServiceMethodBuilder<TService> AddMethodViaFunction<T1, T2, TResult>(Func<TService, T1, T2, TResult> func, MethodDocsOptions opts)
        {
            return AddMethod(opts, () => JsServiceFuncBuilder.GetFunc(func, opts));
        }

        /// <summary>
        /// Добавить метод
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <param name="opts"></param>
        /// <returns></returns>
        public JsWorkerServiceMethodBuilder<TService> AddMethodViaFunction<T1, T2, T3, TResult>(Func<TService, T1, T2, T3, TResult> func, MethodDocsOptions opts)
        {
            return AddMethod(opts, () => JsServiceFuncBuilder.GetFunc(func, opts));
        }

        /// <summary>
        /// Добавить метод
        /// </summary>
        /// <param name="func"></param>
        /// <param name="opts"></param>
        /// <returns></returns>
        public JsWorkerServiceMethodBuilder<TService> AddMethodViaAction(Action<TService> func, MethodDocsOptions opts)
        {
            return AddMethod(opts, () => JsServiceFuncBuilder.GetAction(func, opts));
        }

        /// <summary>
        /// Добавить метод
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="func"></param>
        /// <param name="opts"></param>
        /// <returns></returns>
        public JsWorkerServiceMethodBuilder<TService> AddMethodViaAction<T1>(Action<TService, T1> func, MethodDocsOptions opts)
        {
            return AddMethod(opts, () => JsServiceFuncBuilder.GetAction(func, opts));
        }

        /// <summary>
        /// Добавить метод
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="func"></param>
        /// <param name="opts"></param>
        /// <returns></returns>
        public JsWorkerServiceMethodBuilder<TService> AddMethodViaAction<T1, T2>(Action<TService, T1, T2> func, MethodDocsOptions opts)
        {
            return AddMethod(opts, () => JsServiceFuncBuilder.GetAction(func, opts));
        }

        /// <summary>
        /// Добавить метод
        /// </summary>
        /// <param name="func"></param>
        /// <param name="opts"></param>
        /// <returns></returns>
        public JsWorkerServiceMethodBuilder<TService> AddMethodViaTask(Func<TService, Task> func, MethodDocsOptions opts)
        {
            return AddMethod(opts, () => JsServiceFuncBuilder.GetTask(func, opts));
        }

        /// <summary>
        /// Добавить метод
        /// </summary>
        /// <param name="func"></param>
        /// <param name="opts"></param>
        /// <returns></returns>
        public JsWorkerServiceMethodBuilder<TService> AddMethodViaTask<T1>(Func<TService, T1, Task> func, MethodDocsOptions opts)
        {
            return AddMethod(opts, () => JsServiceFuncBuilder.GetTask(func, opts));
        }

        /// <summary>
        /// Добавить метод
        /// </summary>
        /// <param name="func"></param>
        /// <param name="opts"></param>
        /// <returns></returns>
        public JsWorkerServiceMethodBuilder<TService> AddMethodViaTask<T1, T2>(Func<TService, T1, T2, Task> func, MethodDocsOptions opts)
        {
            return AddMethod(opts, () => JsServiceFuncBuilder.GetTask(func, opts));
        }

        /// <summary>
        /// Добавить метод
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <param name="opts"></param>
        /// <returns></returns>
        public JsWorkerServiceMethodBuilder<TService> AddMethodViaTaskWithResult<TResult>(Func<TService, Task<TResult>> func, MethodDocsOptions opts)
        {
            return AddMethod(opts, () => JsServiceFuncBuilder.GetTask(func, opts));
        }

        /// <summary>
        /// Добавить метод
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <param name="opts"></param>
        /// <returns></returns>
        public JsWorkerServiceMethodBuilder<TService> AddMethodViaTaskWithResult<T1, TResult>(Func<TService, T1, Task<TResult>> func, MethodDocsOptions opts)
        {
            return AddMethod(opts, () => JsServiceFuncBuilder.GetTask(func, opts));
        }

        /// <summary>
        /// Добавить метод
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <param name="opts"></param>
        /// <returns></returns>
        public JsWorkerServiceMethodBuilder<TService> AddMethodViaTaskWithResult<T1, T2, TResult>(Func<TService, T1, T2, Task<TResult>> func, MethodDocsOptions opts)
        {
            return AddMethod(opts, () => JsServiceFuncBuilder.GetTask(func, opts));
        }

        /// <summary>
        /// Построить Js рабочего
        /// </summary>
        public JsWorkerDocumentation Build()
        {
            return Builder.Build();
        }

        private JsWorkerServiceMethodBuilder<TService> AddMethod(MethodDocsOptions opts, Func<JsWorkerMethodDocs> docsGetter)
        {
            if (Builder._docs.Methods.ContainsKey(opts.MethodName))
            {
                throw new InvalidOperationException($"Метод с названием '{opts.MethodName}' уже определен в классе {Builder._docs.WorkerName}");
            }

            var docsPointer = docsGetter();

            Builder._docs.Methods.Add(opts.MethodName, docsPointer);
            return this;
        }
    }
}