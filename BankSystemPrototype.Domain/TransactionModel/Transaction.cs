using BankSystemPrototype.Domain.AccountModel;
using BankSystemPrototype.Domain.BankModel;
using System;

namespace BankSystemPrototype.Domain.TransactionModel
{
    /// <summary>
    /// Банковский перевод
    /// </summary>
    public class Transaction
    {
        private decimal _senderMoneyBefore;
        private decimal _resipientMoneyBefore;
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        public long Id { get; init; }
        /// <summary>
        /// Счет получателя
        /// </summary>
        public Account ResipientAccount { get; init; }
        /// <summary>
        /// Счет отправителя
        /// </summary>
        public Account SenderAccount { get; init; }
        /// <summary>
        /// Деньги
        /// </summary>
        public decimal Money { get; init; }
        /// <summary>
        /// Информация о переводе
        /// </summary>
        public TransactionInfo TransactionInfo { get; init; }
        /// <summary>
        /// Банк, в котором проведена транзакция
        /// </summary>
        public Bank Bank { get; set; }
        public Transaction()
        {
            TransactionInfo = new() { State = TransactionState.New, Message = "" };
        }
        /// <summary>
        /// Совершить транзакцию
        /// </summary>
        public void Begin()
        {
            if (TransactionInfo.State != TransactionState.New) throw new Exception("Невозможно провести транзакцию. Транзакция уже в процессе или завершена.");

            TransactionInfo.State = TransactionState.During;
            _senderMoneyBefore = SenderAccount.Money;
            _resipientMoneyBefore = ResipientAccount.Money;
            try
            {
                if (SenderAccount.SubtractMoney(Money))
                {
                    ResipientAccount.AddMoney(Money);
                    TransactionInfo.State = TransactionState.Successful;
                    TransactionInfo.Message = $"Перевод успешно прошел со счета {SenderAccount.Id} на счет {ResipientAccount.Id} на сумму {Money}";
                }
                else
                {
                    TransactionInfo.State = TransactionState.Error;
                    TransactionInfo.Message = $"Перевод не прошел, недостаточно денег на счете {SenderAccount.Id}";
                }
            }catch(Exception e)
            {
                TransactionInfo.State = TransactionState.Error;
                TransactionInfo.Message = e.Message;
            }
        }
        
        public void RollBack()
        {
            if (TransactionInfo.State != TransactionState.Error) throw new Exception("Невозможно отменить транзакцию. Транзакция новая, в процессе или завершена успешно.");
            var difference = _senderMoneyBefore - SenderAccount.Money;
            if (difference > 0) SenderAccount.AddMoney(difference);
            difference = ResipientAccount.Money - _resipientMoneyBefore;
            if (difference > 0) if (!ResipientAccount.SubtractMoney(difference)) throw new Exception("Не удалось забрать деньги у получателя");
        }
        /// <summary>
        /// Получить информацию о транзакции
        /// </summary>
        /// <returns></returns>
        public TransactionInfo GetInfo() => new TransactionInfo { Message = TransactionInfo.Message, State = TransactionInfo.State };
    }
}