﻿using BankSystemPrototype.Domain.AccountModel;
using BankSystemPrototype.Domain.ClientModel;
using BankSystemPrototype.Domain.TransactionModel;
using BankSystemPrototype.Domain.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystemPrototype.Domain.BankModel
{
    /// <summary>
    /// Банковская система
    /// </summary>
    public class Bank
    {
        /// <summary>
        /// Id банка
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Пользователи банка
        /// </summary>
        private List<User> _users;
        /// <summary>
        /// Пользователи банка
        /// </summary>
        public List<User> Users => _users;
        /// <summary>
        /// Клиенты банка
        /// </summary>
        private List<Client> _clients;
        /// <summary>
        /// Клиенты банка
        /// </summary>
        public IReadOnlyCollection<Client> Clients => _clients;
        /// <summary>
        /// Счета, открыте в банке
        /// </summary>
        private List<Account> _accounts;
        /// <summary>
        /// Счета, открытые в банке
        /// </summary>
        public IReadOnlyCollection<Account> Accounts => _accounts;
        /// <summary>
        /// Транзакции, проведенные в банке
        /// </summary>
        private List<Transaction> _transactions;
        /// <summary>
        /// Транзакции, проведенные в банке
        /// </summary>
        public IReadOnlyCollection<Transaction> Transactions => _transactions;
        /// <summary>
        /// Банковская система
        /// </summary>
        public Bank()
        {
            _clients = new();
            _accounts = new();
            _transactions = new();
        }
        /// <summary>
        /// Банковская система
        /// </summary>
        /// <param name="clients">Клиенты банка</param>
        public Bank(List<Client> clients, List<Transaction> transactions = null) : this()
        {
            _clients.AddRange(clients);
            _clients.ForEach(c => _accounts.AddRange(c.GetAccounts));

            if (transactions is not null)
            {
                _transactions = transactions;
            }
        }
        /// <summary>
        /// Банковская система
        /// </summary>
        /// <param name="accounts">Счета банка</param>
        public Bank(List<Account> accounts, List<Transaction> transactions = null) : this()
        {
            _accounts.AddRange(accounts);
            _accounts.ForEach(a => _clients.Add(a.Owner));
            _clients = _clients.Distinct().ToList();

            if (transactions is not null)
            {
                _transactions = transactions;
            }
        }
        /// <summary>
        /// Добавить физ клиента
        /// </summary>
        /// <param name="firstName">имя</param>
        /// <param name="lastName">фамилия</param>
        public void AddIndividual(string firstName, string lastName)
        {
            _clients.Add(new Individual
            {
                FirstName = firstName,
                LastName = lastName
            });
        }
        /// <summary>
        /// Добавить компанию
        /// </summary>
        /// <param name="inn">ИНН компании</param>
        /// <param name="name">Наименование компании</param>
        public void AddCompany(string inn, string name)
        {
            if (inn.Length != 10 || inn.Length != 12) throw new FormatException("ИНН должен содержать 10 или 12 символов");
            if (!Int64.TryParse(inn, out var longInn)) throw new FormatException("ИНН должен состоять только из цифр");

            _clients.Add(new Company
            {
                INN = inn,
                Name = name
            });
        }
        public void RemoveClient(long clientId)
        {
            var client = _clients.FirstOrDefault(c => c.Id == clientId);
            if (client is null) throw new Exception("Не удалось найти клиента по заданному id");

            _clients.Remove(client);
        }
        /// <summary>
        /// Добавление счета клиенту
        /// </summary>
        /// <param name="clientId">Id клиента</param>
        /// <param name="type">Тип счета</param>
        /// <param name="money">Деньги на счете</param>
        public void AddAccount(long clientId, AccountType type, decimal money = 0)
        {
            var client = _clients.FirstOrDefault(c => c.Id == clientId);
            if (client is null) throw new Exception("Не удалось найти клиента по Id при добавлении счета");

            var account = new Account
            {
                Type = type,
                Owner = client,
                Money = money
            };
            client.AddAccount(account);
        }
        /// <summary>
        /// Удаление счета
        /// </summary>
        /// <param name="accountId"></param>
        public void RemoveAccount(long accountId)
        {
            var account = _accounts.FirstOrDefault(a => a.Id == accountId);
            if (account is null) throw new Exception("Не удалось найти счет по Id при удалении счета");

            // Удаление счета у владельца 
            account.Owner.RemoveAccount(account);
            // Удаление счета из банка
            _accounts.Remove(account);
        }
        public TransactionInfo AddTransaction(long senderAccountId, long resipientAccountId, decimal money)
        {
            if (money < 0) throw new FormatException("Нельзя сделать перевод на отрицательную сумму");

            var sender = Accounts.FirstOrDefault(a => a.Id == senderAccountId);
            if (sender is null) throw new Exception("Не удалось найти счет отправителя по id");

            var resipient = Accounts.FirstOrDefault(a => a.Id == resipientAccountId);
            if (resipient is null) throw new Exception("Не удалось найти счет получателя по id");

            var transaction = new Transaction
            {
                ResipientAccount = resipient,
                SenderAccount = sender
            };
            transaction.Begin();

            if (transaction.GetInfo().State == TransactionState.Error) transaction.RollBack();

            _transactions.Add(transaction);

            return transaction.GetInfo();
        }
    }
}
