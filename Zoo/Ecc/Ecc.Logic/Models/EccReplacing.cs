namespace Ecc.Logic.Models
{
    /// <summary>
    /// Модель описывающая текстовую замену
    /// </summary>
    public class EccReplacing
    {
        /// <summary>
        /// Текст для замены
        /// </summary>
        public string TextToReplace { get; set; }

        /// <summary>
        /// Функция для замены
        /// </summary>
        public EccTextFunc Func { get; set; }
    }
}