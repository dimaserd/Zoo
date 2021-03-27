namespace Clt.Contract.Events
{
    /// <summary>
    /// Клиент изменил пароль
    /// </summary>
    public class ClientChangedPassword
    {
        /// <summary>
        /// Идентификатор клиента
        /// </summary>
        public string ClientId { get; set; }
    }
}