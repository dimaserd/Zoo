namespace Clt.Contract.Enumerations
{
    /// <summary>
    /// Действие, которое должно быть вызвано при различии в пользовательских данных
    /// </summary>
    public enum UserDifferenceAction
    {
        /// <summary>
        /// Ничего не делать
        /// </summary>
        None,

        /// <summary>
        /// Перелогиниться автома
        /// </summary>
        AutoReLogin,

        /// <summary>
        /// 
        /// </summary>
        Logout
    }
}