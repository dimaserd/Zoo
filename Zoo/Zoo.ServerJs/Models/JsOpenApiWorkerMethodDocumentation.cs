﻿using Croco.Core.Documentation.Models;
using System.Collections.Generic;
using System.Linq;

namespace Zoo.ServerJs.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class JsOpenApiWorkerMethodDocumentation
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="methodDocs"></param>
        public JsOpenApiWorkerMethodDocumentation(JsWorkerMethodDocs methodDocs)
        {
            MethodName = methodDocs.MethodName;
            Description = methodDocs.Description;

            if(methodDocs.Response != null)
            {
                Response = CrocoTypeDescription.GetDescription(methodDocs.Response);
            }
            
            if(methodDocs.Parameters != null)
            {
                Parameters = methodDocs.Parameters.Select(x => CrocoTypeDescription.GetDescription(x)).ToList();
            }
        }

        /// <summary>
        /// Название метода
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Ответ от метода обработчика. Подробное описание типа.
        /// </summary>
        public CrocoTypeDescription Response { get; set; }

        /// <summary>
        /// Параметры для метода обработчика. Подробное описание типа.
        /// </summary>
        public List<CrocoTypeDescription> Parameters { get; set; }
    }
}