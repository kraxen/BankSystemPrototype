namespace BankSystemPrototype.Domain.TransactionModel
{
    /// <summary>
    /// Состояния перевода
    /// </summary>
    public enum TransactionState
    {
        New,
        During,
        Successful,
        Error
    }
}