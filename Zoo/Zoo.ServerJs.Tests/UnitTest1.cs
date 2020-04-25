using Croco.Core.Abstractions.Models;
using NUnit.Framework;
using System.Collections.Generic;
using Zoo.ServerJs.Abstractions;
using Zoo.ServerJs.Models;
using Zoo.ServerJs.Services;

namespace Zoo.ServerJs.Tests
{
    public class ProductInProductGroupIdModel
    {
        public string ProductGroupId { get; set; }

        public int ProductId { get; set; }
    }

    public class ProductGroupJsWorker : IJsWorker
    {
        static BaseApiResponse AddProductToGroup(ProductInProductGroupIdModel model)
        {
            return new BaseApiResponse(true, "ok");
        }

        public JsWorkerDocumentation JsWorkerDocs()
        {
            return new JsWorkerDocumentation
            {
                WorkerName = "ProductGroupJsWorker",
                Description = "",
                Methods = new List<JsWorkerMethodDocs>
                {
                    JsWorkerMethodDocs.GetMethod(new JsWorkerMethodDocsOptions
                    {
                        MethodName = "AddProductToGroup",
                        Description = "Добавить товар в группу товаров",
                    }, new JsFunc<ProductInProductGroupIdModel, BaseApiResponse>(AddProductToGroup))
                }
            };
        }
    }

    public class Tests
    {
        [Test]
        public void Test1()
        {
            var script = "api.Call(\"ProductGroupJsWorker\", \"AddProductToGroup\", { ProductGroupId: \"d8c8cf9b-1d9b-4199-a85e-615edd64b4d7\", ProductId: 1 });";

            var executor = new JsExecutor(new JsExecutorProperties
            {
                JsWorkers = new List<IJsWorker>
                {
                    new ProductGroupJsWorker()
                }
            });

            executor.RunScriptDetaiiled(script);
        }
    }
}