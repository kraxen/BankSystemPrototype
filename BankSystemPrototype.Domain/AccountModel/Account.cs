using BankSystemPrototype.Domain.ClientModel;
using System;

namespace BankSystemPrototype.Domain.AccountModel
{
    /// <summary>
    /// Счет в банке
    /// </summary>
    public class Account
    {
        /// <summary>
        /// Номер счета, уникальный
        /// </summary>
        public long Id { get; init; }
        /// <summary>
        /// Тип счета
        /// </summary>
        public AccountType Type { get; init; }
        /// <summary>
        /// Деньги на счете
        /// </summary>
        private decimal _money;
        /// <summary>
        /// Деньги на счете
        /// </summary>
        public decimal Money { get => _money; init { _money = value; } }
        /// <summary>
        /// Владелец счета
        /// </summary>
        public Client Owner { get; init; }
        /// <summary>
        /// Процентная ставка
        /// </summary>
        public decimal Persent 
        {
            get
            {
                if (Type == AccountType.Account) return 0;
                if (Type == AccountType.Deposit)
                {
                    if (Owner.GetType() == typeof(Individual)) return 6;
                    else return 10;
                }
                else
                {
                    if (Owner.GetType() == typeof(Individual)) return 15;
                    else return 26;
                }
            }
        }
        /// <summary>
        /// Добавить деньги на счет
        /// </summary>
        /// <param name="money"></param>
        /// <returns></returns>
        public bool AddMoney(decimal money)
        {
            if (money < 0) throw new FormatException("Количество денег было меньше нуля");

            _money += money;
            return true;
        }
        /// <summary>
        /// Убрать деньги на счете
        /// </summary>
        /// <param name="money"></param>
        /// <returns></returns>
        public bool SubtractMoney(decimal money)
        {
            if (money < 0) throw new FormatException("Количество денег было меньше нуля");

            if (_money < money) return false;
            _money -= money;
            return true;
        }
    }
}