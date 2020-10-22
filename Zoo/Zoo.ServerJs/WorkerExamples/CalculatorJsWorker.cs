using Zoo.ServerJs.Abstractions;
using Zoo.ServerJs.Models.Method;
using Zoo.ServerJs.Services;

namespace Zoo.ServerJs.WorkerExamples
{
    /// <summary>
    /// Калькулятор
    /// </summary>
    public class CalculatorJsWorker : IJsWorker
    {
        private double Divide(double a1, double a2)
        {
            return a1 / a2;
        }

        private double Multiply(double a1, double a2)
        {
            return a1 * a2;
        }

        private double Add(double a1, double a2)
        {
            return a1 + a2;
        }

        private double Substract(double a1, double a2)
        {
            return a1 - a2;
        }

        /// <inheritdoc />
        public JsWorkerDocumentation JsWorkerDocs(JsWorkerBuilder builder)
        {
            return builder.SetWorkerName("Calculator")
                .SetDescription("Калькулятор")
                .AddMethodViaFunction<double, double, double>(Divide, new JsWorkerMethodDocsOptions
                {
                    MethodName = "Divide",
                    Description = "Разделить",
                    ParameterDescriptions = new[] { "Делимое", "Делитель" },
                    ResultDescription = "Результат деления",
                })
                .AddMethodViaFunction<double, double, double>(Multiply, new JsWorkerMethodDocsOptions
                {
                    MethodName = "Multiply",
                    Description = "Умножить",
                    ParameterDescriptions = new[] { "Множитель 1", "Множитель 2" },
                    ResultDescription = "Результат умножения",
                })
                .AddMethodViaFunction<double, double, double>(Add, new JsWorkerMethodDocsOptions
                {
                    MethodName = "Add",
                    Description = "Сложить",
                    ParameterDescriptions = new[] { "Слагаемое 1", "Слагаемое 2" },
                    ResultDescription = "Результат сложения",
                })
                .AddMethodViaFunction<double, double, double>(Substract, new JsWorkerMethodDocsOptions
                {
                    MethodName = "Substract",
                    Description = "Вычесть",
                    ParameterDescriptions = new[] { "Из кого вычесть", "Что вычесть" },
                    ResultDescription = "Результат вычитания",
                }).Build();
        }
    }
}