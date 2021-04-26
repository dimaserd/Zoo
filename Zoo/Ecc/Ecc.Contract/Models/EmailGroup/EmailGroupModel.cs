namespace Ecc.Contract.Models.EmailGroup
{
    /// <summary>
    /// Группа эмейлов
    /// </summary>
    public class EmailGroupModel
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Количество эмейлов
        /// </summary>
        public int EmailsCount { get; set; }
    }
}