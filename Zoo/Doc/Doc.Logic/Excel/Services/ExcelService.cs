using ClosedXML.Excel;
using Doc.Logic.Excel.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace Doc.Logic.Excel.Services
{
    /// <summary>
    /// Сервис для работы с Excel
    /// </summary>
    public static class ExcelService
    {
        /// <summary>
        /// Перекладывает первый лист из таблицы Excel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="excelFilePath"></param>
        /// <returns></returns>
        public static List<T> ExcelToList<T>(string excelFilePath) where T : class, new()
        {
            List<Dictionary<string, object>> listOfDicts;

            using (var dt = ExcelToDataTable(excelFilePath))
            {
                listOfDicts = ExcelExtensions.ToDictionaryList(dt);
            }

            var descriptions = ExcelExtensions.GetMyPropertyDescriptions(typeof(T));

            foreach (var dict in listOfDicts)
            {
                foreach (var description in descriptions)
                {
                    if (!dict.ContainsKey(description.DisplayName))
                    {
                        continue;
                    }

                    dict[description.PropertyName] = dict[description.DisplayName];
                    dict.Remove(description.DisplayName);
                }
            }

            return listOfDicts.Select(x => x.ToObject<T>()).ToList();
        }

        /// <summary>
        /// Сохранить лист из объектов в файл Excel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="filePath"></param>
        /// <param name="tableName"></param>
        /// <param name="fromDisplayAttr"></param>
        public static void SaveListToExcel<T>(IList<T> list, string filePath, string tableName = null, bool fromDisplayAttr = false)
        {
            var dt = list.ToDataTable(tableName, fromDisplayAttr);

            SaveDataTableToExcel(dt, filePath);
        }

        /// <summary>
        /// Сохранить DataTable в файл Excel
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="filePath"></param>
        public static void SaveDataTableToExcel(DataTable dt, string filePath)
        {
            using var ds = new DataSet();
            ds.Tables.Add(dt);

            SaveDataSetToExcel(ds, filePath);
        }

        /// <summary>
        /// Сохранить датасет в файл эксель
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="filePath"></param>
        public static void SaveDataSetToExcel(DataSet ds, string filePath)
        {
            var ext = Path.GetExtension(filePath);

            if (ext != ".xlsx")
            {
                throw new Exception("Недопустимое разрешение файла");
            }

            using var wb = new XLWorkbook();
            foreach (DataTable dt in ds.Tables)
            {
                wb.Worksheets.Add(dt, dt.TableName);
            }

            wb.SaveAs(filePath);
        }

        /// <summary>
        /// Прочитать файл по пути и перевести его в DataTable
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static DataTable ExcelToDataTable(string filePath)
        {
            //Open the Excel file using ClosedXML.
            using var workBook = new XLWorkbook(filePath);
            //Read the first Sheet from Excel file.
            var workSheet = workBook.Worksheet(1);

            //Create a new DataTable.
            var dt = new DataTable();

            //Loop through the Worksheet rows.
            var firstRow = true;
            foreach (var row in workSheet.Rows())
            {
                //Use the first row to add columns to DataTable.
                if (firstRow)
                {
                    foreach (var cell in row.Cells())
                    {
                        dt.Columns.Add(cell.Value.ToString());
                    }
                    firstRow = false;
                }
                else
                {
                    //Add rows to DataTable.
                    dt.Rows.Add();
                    var i = 0;
                    foreach (var cell in row.Cells())
                    {
                        dt.Rows[^1][i] = cell.Value.ToString();
                        i++;
                    }
                }
            }

            return dt;
        }
    }
}