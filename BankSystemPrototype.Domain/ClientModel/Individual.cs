namespace BankSystemPrototype.Domain.ClientModel
{
    /// <summary>
    /// Физ клиент банка
    /// </summary>
    public class Individual : Client
    {
        /// <summary>
        /// Имя клиента
        /// </summary>
        public string FirstName { get; init; }
        /// <summary>
        /// Фамилия клиента
        /// </summary>
        public string LastName { get; init; }
    }
}