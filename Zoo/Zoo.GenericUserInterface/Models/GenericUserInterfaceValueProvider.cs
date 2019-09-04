using Croco.Core.Common.Utils;
using Croco.Core.Documentation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Zoo.GenericUserInterface.Models.Values;

namespace Zoo.GenericUserInterface.Models
{
    /// <summary>
    /// Провайдер значений для модели
    /// </summary>
    public class GenericUserInterfaceValueProvider
    {
        /// <summary>
        /// Значения для типов данных не являющихся массивами
        /// </summary>
        public List<GenericUserInterfacePropertySingleValue> Singles { get; set; }

        /// <summary>
        /// Значения для типов данных которые являются массивами
        /// </summary>
        public List<GenericUserInterfacePropertyListValue> Arrays { get; set; }

        /// <summary>
        /// Создать провайдера значений из объекта
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static GenericUserInterfaceValueProvider Create(object model)
        {
            var modelType = model.GetType();

            var typeDescription = CrocoTypeDescription.GetDescription(modelType);

            if (!typeDescription.IsClass)
            {
                throw new Exception("Не поддерживается");
            }

            var t = new GenericUserInterfaceValueProvider
            {
                Singles = new List<GenericUserInterfacePropertySingleValue>(),
                Arrays = new List<GenericUserInterfacePropertyListValue>()
            };


            foreach (var prop in typeDescription.Properties)
            {
                if (prop.IsClass)
                {
                    throw new Exception("Вложенные классы не поддерживаются");
                }

                var jsonValueOfModel = GetJson(model, prop);

                if(prop.IsEnumerable)
                {
                    t.Arrays.Add(new GenericUserInterfacePropertyListValue
                    {
                        PropertyName = prop.PropertyName,
                        Value = Tool.JsonConverter.Deserialize<List<string>>(jsonValueOfModel)
                    });
                }
                else
                {
                    t.Singles.Add(new GenericUserInterfacePropertySingleValue
                    {
                        PropertyName = prop.PropertyName,
                        Value = Tool.JsonConverter.Deserialize<string>(jsonValueOfModel)
                    });
                }
            }

            return t;
        }

        private static string GetJson(object obj, CrocoTypeDescription prop)
        {
            var propOfModel = obj.GetType().GetProperty(prop.PropertyName);

            var valueOfProp = propOfModel.GetValue(obj);

            return prop.IsEnumerable ? UnWrapJsonForArrayOfSomething(valueOfProp) : Tool.JsonConverter.Serialize(valueOfProp);
        }

        private static string UnWrapJsonForArrayOfSomething(object valueOfProp)
        {
            var init = Tool.JsonConverter.Deserialize<List<object>>(Tool.JsonConverter.Serialize(valueOfProp));

            //Сериализую элементы массива
            var serializedData = init.Select(x => Tool.JsonConverter.Serialize(x)).ToList();

            return Tool.JsonConverter.Serialize(serializedData);
        }
    }
}
