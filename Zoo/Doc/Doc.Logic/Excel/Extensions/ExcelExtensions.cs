using Doc.Logic.Excel.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace Doc.Logic.Excel.Extensions
{
    /// <summary>
    /// Расширения для Экселя
    /// </summary>
    public static class ExcelExtensions
    {
        /// <summary>
        /// Привести DataTable к листу из словарей, где словарь это один из объектов
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<Dictionary<string, object>> ToDictionaryList(DataTable dataTable)
        {
            var list = new List<Dictionary<string, object>>();

            foreach (DataRow row in dataTable.Rows)
            {
                var dict = new Dictionary<string, object>();

                foreach (var column in dataTable.Columns)
                {
                    var columnName = column.ToString();

                    var value = row[columnName];

                    dict[columnName] = value;
                }

                list.Add(dict);
            }

            return list;
        }

        /// <summary>
        /// Получить описание для типа
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<MyPropertyDescription> GetMyPropertyDescriptions(Type type)
        {
            var result = new List<MyPropertyDescription>();

            var props = TypeDescriptor.GetProperties(type);

            foreach (PropertyDescriptor prop in props)
            {
                result.Add(new MyPropertyDescription
                {
                    DisplayName = GetDisplayNameForProperty(prop),
                    PropertyName = prop.Name,
                    Type = prop.PropertyType
                });
            }

            return result;
        }

        /// <summary>
        /// Перевести список данных к DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="tableName"></param>
        /// <param name="displayFromAttr"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IList<T> data, string tableName = null, bool displayFromAttr = false)
        {
            var props = TypeDescriptor.GetProperties(typeof(T));

            var table = new DataTable
            {
                TableName = tableName ?? typeof(T).Name
            };

            for (var i = 0; i < props.Count; i++)
            {
                var prop = props[i];

                var propName = displayFromAttr ? GetDisplayNameForProperty(prop) : prop.Name;

                table.Columns.Add(propName, prop.PropertyType);
            }

            var values = new object[props.Count];

            foreach (var item in data)
            {
                for (var i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }

        private static string GetDisplayNameForProperty(PropertyDescriptor prop)
        {
            if (prop == null)
            {
                throw new ArgumentNullException(nameof(prop));
            }

            var display = prop.Attributes.OfType<DisplayAttribute>().FirstOrDefault();

            if (display == null)
            {
                throw new NullReferenceException($"Атрибут Display не указан на свойстве {prop.Name}");
            }

            return display.Name;
        }
    }
}
