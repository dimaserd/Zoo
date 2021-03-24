using Croco.Core.Contract.Files;

namespace Clt.Logic.Abstractions
{
    /// <summary>
    /// Проверщик для файлов на изображения
    /// </summary>
    public interface IFileImageChecker
    {
        /// <summary>
        /// Является ли файл изображением
        /// </summary>
        /// <param name="fileData"></param>
        /// <returns></returns>
        bool IsImage(IFileData fileData);
    }
}
