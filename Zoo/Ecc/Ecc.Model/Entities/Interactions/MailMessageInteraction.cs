namespace Ecc.Model.Entities.Interactions
{
    /// <summary>
    /// Сущность описывающая взаимодействие по почте
    /// </summary>
    public class MailMessageInteraction : Interaction
    {
        /// <summary>
        /// Куда было отправлено сообщение
        /// </summary>
        public string ReceiverEmail { get; set; }
    }
}