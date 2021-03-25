namespace Doc.Logic.Word.Models
{
    /// <summary>
    /// Модель для вставки изображения вместо указанного текста
    /// </summary>
    public class DocxImageReplace
    {
        /// <summary>
        /// Текст, который будет заменен изображением
        /// </summary>
        public string TextToReplace { get; set; }

        /// <summary>
        /// Путь к изображению, который будет вставлен вместо текста
        /// </summary>
        public string ImageFilePath { get; set; }
    }
}