using Doc.Logic.Excel.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Doc.Logic.Excel.Extensions
{
    /// <summary>
    /// Расширения для работы с объектами
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Привести словарь к объекту
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T ToObject<T>(this IDictionary<string, object> source)
            where T : class, new()
        {
            var someObject = new T();
            var someObjectType = someObject.GetType();

            foreach (var item in source)
            {
                var prop = someObjectType
                    .GetProperty(item.Key);

                if (prop == null)
                {
                    throw new Exception($"Не найдено свойство {item.Key} в объекте типа {typeof(T).FullName}");
                }

                if (prop.PropertyType == typeof(decimal))
                {
                    var value = decimal.Parse(item.Value.ToString().Replace(",", "."), CultureInfo.InvariantCulture);
                    prop.SetValue(someObject, value, null);
                }
                else if (prop.PropertyType == typeof(bool))
                {
                    var arrayOfTrue = new[]
                    {
                        MainResource.Yes, MainResource.Truth,
                    };
                    var value = arrayOfTrue.Contains(item.Value.ToString().ToLower());

                    prop.SetValue(someObject, value, null);
                }
                else if (prop.PropertyType == typeof(int))
                {
                    var value = item.Value.ToString();

                    if (!int.TryParse(value, out var intValue))
                    {
                        throw new Exception($"Не удалось распознать тип данных 'Целое число' из значения '{value}'");
                    }

                    prop.SetValue(someObject, intValue, null);
                }
                else if (prop.PropertyType == typeof(DateTime))
                {
                    var value = item.Value.ToString();

                    if (!DateTime.TryParse(value, CultureInfo.InvariantCulture,
                       DateTimeStyles.None, out var dateTime))
                    {
                        throw new Exception($"Не удалось распознать тип данных 'Дата и время' из значения '{value}'");
                    }

                    prop.SetValue(someObject, dateTime, null);
                }
                else
                {
                    prop.SetValue(someObject, item.Value, null);
                }
            }

            return someObject;
        }

        /// <summary>
        /// Приветсти объект к словарю
        /// </summary>
        /// <param name="source"></param>
        /// <param name="bindingAttr"></param>
        /// <returns></returns>
        public static Dictionary<string, object> AsDictionary(this object source, BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
        {
            return source.GetType().GetProperties(bindingAttr).ToDictionary
            (
                propInfo => propInfo.Name,
                propInfo => propInfo.GetValue(source, null)
            );
        }
    }
}