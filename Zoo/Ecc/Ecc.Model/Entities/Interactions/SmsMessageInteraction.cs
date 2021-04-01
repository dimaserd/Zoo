namespace Ecc.Model.Entities.Interactions
{
    /// <summary>
    /// Смс взаимодействие
    /// </summary>
    public class SmsMessageInteraction : Interaction
    {
        /// <summary>
        /// Номер телефона
        /// </summary>
        public string PhoneNumber { get; set; }
    }
}