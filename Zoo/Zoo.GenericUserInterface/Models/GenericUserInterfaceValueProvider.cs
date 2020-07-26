using Croco.Core.Documentation.Models;
using Croco.Core.Utils;
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

            var main = typeDescription.GetMainTypeDescription();

            if (!main.IsClass)
            {
                throw new Exception("Не поддерживается");
            }

            var t = new GenericUserInterfaceValueProvider
            {
                Singles = new List<GenericUserInterfacePropertySingleValue>(),
                Arrays = new List<GenericUserInterfacePropertyListValue>()
            };


            foreach (var prop in main.Properties)
            {
                var propDescr = typeDescription.GetTypeDescription(prop.DisplayFullTypeName);

                if (propDescr.IsClass)
                {
                    //Вложенный класс не поддерживается как провайдер значений для свойства
                    continue;
                }

                var jsonValueOfModel = GetJson(model, prop, typeDescription);

                if(propDescr.IsEnumerable)
                {
                    t.Arrays.Add(new GenericUserInterfacePropertyListValue
                    {
                        PropertyName = prop.PropertyDescription.PropertyName,
                        Value = Tool.JsonConverter.Deserialize<List<string>>(jsonValueOfModel)
                    });
                }
                else
                {
                    t.Singles.Add(new GenericUserInterfacePropertySingleValue
                    {
                        PropertyName = prop.PropertyDescription.PropertyName,
                        Value = Tool.JsonConverter.Deserialize<string>(jsonValueOfModel)
                    });
                }
            }

            return t;
        }

        private static string GetJson(object obj, CrocoPropertyReferenceDescription prop, CrocoTypeDescriptionResult main)
        {
            var propDescr = main.GetTypeDescription(prop.DisplayFullTypeName);

            var propOfModel = obj.GetType().GetProperty(prop.PropertyDescription.PropertyName);

            var valueOfProp = propOfModel.GetValue(obj);

            return propDescr.IsEnumerable ? UnWrapJsonForArrayOfSomething(valueOfProp) : Tool.JsonConverter.Serialize(valueOfProp);
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