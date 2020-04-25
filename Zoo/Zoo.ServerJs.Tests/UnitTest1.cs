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

        public static int[] GetArray()
        {
            return new int[4] { 1, 2, 3, 4 };
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
                    }, new JsFunc<ProductInProductGroupIdModel, BaseApiResponse>(AddProductToGroup)),

                    JsWorkerMethodDocs.GetMethod(new JsWorkerMethodDocsOptions
                    {
                        MethodName = "GetArray",
                        Description = "Получить массив"
                    }, new JsFunc<int[]>(GetArray))
                }
            };
        }
    }

    public class Tests
    {
        [Test]
        public void Test1()
        {
            var script = "var t = api.Call(\"ProductGroupJsWorker\", \"AddProductToGroup\", { ProductGroupId: \"d8c8cf9b-1d9b-4199-a85e-615edd64b4d7\", ProductId: 1 });";

            script += "\n console.log('Result', t)";

            var executor = new JsExecutor(new JsExecutorProperties
            {
                JsWorkers = new List<IJsWorker>
                {
                    new ProductGroupJsWorker()
                }
            });

            var result = executor.RunScriptDetaiiled(script);
        }

        [Test]
        public void Test2()
        {
            var script = "var t = JSON.parse( api.Call(\"ProductGroupJsWorker\", \"GetArray\") );\n";

            script += "console.log(t);\n";
            script += "console.log(t.length);\n";
            script += "for(var i = 0; i < t.length; i++) {";
            
            script += "\n console.log('Result', i, t[i]); \n}";

            var executor = new JsExecutor(new JsExecutorProperties
            {
                JsWorkers = new List<IJsWorker>
                {
                    new ProductGroupJsWorker()
                }
            });

            var result = executor.RunScriptDetaiiled(script);
        }

        [Test]
        public void Test3()
        {
            var script = $"var t = {ProductGroupJsWorker.GetArray()};\n";

            script += "console.log(t);\n";
            script += "console.log(t.length);\n";
            script += "for(var i = 0; i < t.length; i++) {";

            script += "\n console.log('Result', i, t[i]); \n}";

            var executor = new JsExecutor(new JsExecutorProperties
            {
                JsWorkers = new List<IJsWorker>
                {
                    new ProductGroupJsWorker()
                }
            });

            var result = executor.RunScriptDetaiiled(script);
        }
    }
}