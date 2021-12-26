using BankSystemPrototype.Domain.AccountModel;
using BankSystemPrototype.Domain.BankModel;
using System;
using System.Collections.Generic;

namespace BankSystemPrototype.Domain.ClientModel
{
    /// <summary>
    /// Клиент банка
    /// </summary>
    public abstract class Client
    {
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        public long Id { get; init; }
        /// <summary>
        /// Счета клиента
        /// </summary>
        private List<Account> Accounts { get; set; } = new();
        public IReadOnlyCollection<Account> GetAccounts => Accounts;
        /// <summary>
        /// Добавление счета
        /// </summary>
        /// <param name="account"></param>
        public void AddAccount(Account account)
        {
            if (account.Owner != this) throw new Exception("Попытка добавить чужой счет");
            Accounts.Add(account);
        }
        /// <summary>
        /// Банк, в котором находится клиент
        /// </summary>
        public Bank Bank { get; set; }
        /// <summary>
        /// Удаление счета
        /// </summary>
        /// <param name="account"></param>
        public void RemoveAccount(Account account)
        {
            if (!Accounts.Contains(account)) throw new Exception("Попытка удалить счет, которого нет у клиента"); 
            Accounts.Remove(account);
        }
    }
}