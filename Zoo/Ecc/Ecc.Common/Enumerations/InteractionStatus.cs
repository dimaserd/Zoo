namespace Ecc.Common.Enumerations
{
    /// <summary>
    /// Статус взаимодействия
    /// </summary>
    public enum InteractionStatus
    {
        /// <summary>
        /// Создано
        /// </summary>
        Created = 0,

        /// <summary>
        /// В процессе отправки
        /// </summary>
        InProccess = 1,

        /// <summary>
        /// Статус пока неизвестен. (Нужно проверить у провайдера)
        /// </summary>
        Sent = 2,

        /// <summary>
        /// Успешно
        /// </summary>
        Delivered = 3,

        /// <summary>
        /// Ошибка
        /// </summary>
        Error = 4
    }
}