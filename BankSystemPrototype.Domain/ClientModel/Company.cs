namespace BankSystemPrototype.Domain.ClientModel
{
    /// <summary>
    /// Компания клиент банка
    /// </summary>
    public class Company : Client
    {
        /// <summary>
        /// ИНН компании
        /// </summary>
        public string INN { get; init; }
        /// <summary>
        /// Наименование компании
        /// </summary>
        public string Name { get; init; }
    }
}