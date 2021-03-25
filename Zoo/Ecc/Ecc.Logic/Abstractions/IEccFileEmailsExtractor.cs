using System.Collections.Generic;
using System.Threading.Tasks;
using Croco.Core.Contract.Models;

namespace Ecc.Logic.Abstractions
{
    public interface IEccFileEmailsExtractor
    {
        /// <summary>
        /// Существует ли файл
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        bool FileExists(string filePath);

        /// <summary>
        /// Получить список emailов из файла 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        Task<BaseApiResponse<List<string>>> ExtractEmailsListFromFile(string filePath);
    }
}