namespace BankSystemPrototype.Domain.TransactionModel
{
    public class TransactionInfo
    {
        public long Id { get; set; }
        /// <summary>
        /// Состояние перевода
        /// </summary>
        public TransactionState State { get; set; }
        /// <summary>
        /// Подробности транзакции
        /// </summary>
        public string Message { get; set; }
    }
}