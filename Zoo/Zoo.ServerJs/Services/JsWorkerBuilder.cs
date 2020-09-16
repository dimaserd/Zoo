﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zoo.ServerJs.Models.Method;

namespace Zoo.ServerJs.Services
{
    /// <summary>
    /// Построитель Js рабочего
    /// </summary>
    public class JsWorkerBuilder
    {
        internal readonly JsWorkerDocumentation _docs = new JsWorkerDocumentation
        {
            WorkerName = null,
            Description = null,
            Methods = new Dictionary<string, JsWorkerMethodDocs>()
        };

        JsExecutorBuilder ExecutorBuilder { get; }

        internal JsWorkerBuilder(JsExecutorBuilder executorBuilder)
        {
            ExecutorBuilder = executorBuilder;
        }

        /// <summary>
        /// Установить название
        /// </summary>
        /// <param name="workerName"></param>
        /// <returns></returns>
        public JsWorkerBuilder SetWorkerName(string workerName)
        {
            _docs.WorkerName = workerName;
            return this;
        }

        /// <summary>
        /// Установить описание
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public JsWorkerBuilder SetDescription(string description)
        {
            _docs.Description = description;
            return this;
        }

        /// <summary>
        /// Получить построителя метода относительно сервиса
        /// </summary>
        /// <typeparam name="TService">Тип сервиса</typeparam>
        /// <returns></returns>
        public JsWorkerServiceMethodBuilder<TService> GetServiceMethodBuilder<TService>()
        {
            return new JsWorkerServiceMethodBuilder<TService>(this, ExecutorBuilder.ServiceCollection);
        }

        /// <summary>
        /// Добавить метод
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <param name="opts"></param>
        /// <returns></returns>
        public JsWorkerBuilder AddMethod<TResult>(Func<TResult> func, JsWorkerMethodDocsOptions opts)
        {
            return AddMethod(opts, () => JsFuncBuilder.GetFunc(func, opts));
        }

        /// <summary>
        /// Добавить метод
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <param name="opts"></param>
        /// <returns></returns>
        public JsWorkerBuilder AddMethod<T1, TResult>(Func<T1, TResult> func, JsWorkerMethodDocsOptions opts)
        {
            return AddMethod(opts, () => JsFuncBuilder.GetFunc(func, opts));
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
        public JsWorkerBuilder AddMethod<T1, T2, TResult>(Func<T1, T2, TResult> func, JsWorkerMethodDocsOptions opts)
        {
            return AddMethod(opts, () => JsFuncBuilder.GetFunc(func, opts));
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
        public JsWorkerBuilder AddMethod<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> func, JsWorkerMethodDocsOptions opts)
        {
            return AddMethod(opts, () => JsFuncBuilder.GetFunc(func, opts));
        }

        /// <summary>
        /// Добавить метод
        /// </summary>
        /// <param name="func"></param>
        /// <param name="opts"></param>
        /// <returns></returns>
        public JsWorkerBuilder AddMethod(Action func, JsWorkerMethodDocsOptions opts)
        {
            return AddMethod(opts, () => JsFuncBuilder.GetAction(func, opts));
        }

        /// <summary>
        /// Добавить метод
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="func"></param>
        /// <param name="opts"></param>
        /// <returns></returns>
        public JsWorkerBuilder AddMethod<T1>(Action<T1> func, JsWorkerMethodDocsOptions opts)
        {
            return AddMethod(opts, () => JsFuncBuilder.GetAction(func, opts));
        }

        /// <summary>
        /// Добавить метод
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="func"></param>
        /// <param name="opts"></param>
        /// <returns></returns>
        public JsWorkerBuilder AddMethod<T1, T2>(Action<T1, T2> func, JsWorkerMethodDocsOptions opts)
        {
            return AddMethod(opts, () => JsFuncBuilder.GetAction(func, opts));
        }

        /// <summary>
        /// Добавить метод
        /// </summary>
        /// <param name="func"></param>
        /// <param name="opts"></param>
        /// <returns></returns>
        public JsWorkerBuilder AddMethod(Func<Task> func, JsWorkerMethodDocsOptions opts)
        {
            return AddMethod(opts, () => JsFuncBuilder.GetTask(func, opts));
        }

        /// <summary>
        /// Добавить метод
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="func"></param>
        /// <param name="opts"></param>
        /// <returns></returns>
        public JsWorkerBuilder AddMethod<T1>(Func<T1, Task> func, JsWorkerMethodDocsOptions opts)
        {
            return AddMethod(opts, () => JsFuncBuilder.GetTask(func, opts));
        }

        /// <summary>
        /// Добавить метод
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <param name="opts"></param>
        /// <returns></returns>
        public JsWorkerBuilder AddMethod<TResult>(Func<Task<TResult>> func, JsWorkerMethodDocsOptions opts)
        {
            return AddMethod(opts, () => JsFuncBuilder.GetTask(func, opts));
        }

        /// <summary>
        /// Добавить метод
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <param name="opts"></param>
        /// <returns></returns>
        public JsWorkerBuilder AddMethod<T1, TResult>(Func<T1, Task<TResult>> func, JsWorkerMethodDocsOptions opts)
        {
            return AddMethod(opts, () => JsFuncBuilder.GetTask(func, opts));
        }


        /// <summary>
        /// Построить Js рабочего
        /// </summary>
        /// <returns></returns>
        public JsWorkerDocumentation Build()
        {
            _docs.Validate();
            return _docs;
        }

        private JsWorkerBuilder AddMethod(JsWorkerMethodDocsOptions opts, Func<JsWorkerMethodDocs> docsGetter)
        {
            if (_docs.Methods.ContainsKey(opts.MethodName))
            {
                throw new InvalidOperationException($"Метод с названием '{opts.MethodName}' уже определен в классе {_docs.WorkerName}");
            }

            var docsPointer = docsGetter();

            _docs.Methods.Add(opts.MethodName, docsPointer);
            return this;
        }
    }
}