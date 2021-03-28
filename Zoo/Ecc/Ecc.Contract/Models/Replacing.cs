namespace Ecc.Contract.Models
{
    /// <summary>
    /// Модель описывающая замену по ключу
    /// </summary>
    public class Replacing
    {
        /// <summary>
        /// Ключ который будет заменен
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Значение, на которое будет произведена замена
        /// </summary>
        public string Value { get; set; }
    }
}