namespace Ecc.Logic.Models.EmailTemplates
{
    /// <summary>
    /// Редактирование шаблона эмейла
    /// </summary>
    public class EditEmailTemplate : CreateEmailTemplate
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public string Id { get; set; }
    }
}