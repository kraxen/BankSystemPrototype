using BankSystemPrototype.Domain.AccountModel;
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
        private long _lastUserId = 0;
        private long _lastClientId = 0;
        private long _lastAccountId = 0;
        private long _lastTransactionId = 0;
        /// <summary>
        /// Id банка
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Название банка
        /// </summary>
        public string Name { get; init; }
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
            _users = new();
            Name = String.Empty;
        }
        /// <summary>
        /// Банковская система
        /// </summary>
        /// <param name="clients">Клиенты банка</param>
        public Bank(List<User> users, List<Client> clients, List<Transaction> transactions = null) : this()
        {
            _clients.AddRange(clients);
            _clients.ForEach(c => _accounts.AddRange(c.GetAccounts));

            _lastClientId = _clients.Max(c => c.Id);
            _lastAccountId = _accounts.Max(a => a.Id);

            if (transactions is not null)
            {
                _transactions = transactions;
                _lastTransactionId = _transactions.Max(t => t.Id);
            }
            if(users is not null)
            {
                _users = users;
                _lastUserId = _users.Max(u => u.Id);
            }
            Name = String.Empty;
        }
        /// <summary>
        /// Банковская система
        /// </summary>
        /// <param name="accounts">Счета банка</param>
        public Bank(List<User> users, List<Account> accounts, List<Transaction> transactions = null) : this()
        {
            _accounts.AddRange(accounts);
            _accounts.ForEach(a => _clients.Add(a.Owner));
            _clients = _clients.Distinct().ToList();

            _lastClientId = _clients.Max(c => c.Id);
            _lastAccountId = _accounts.Max(a => a.Id);

            if (transactions is not null)
            {
                _transactions = transactions;
                _lastTransactionId = _transactions.Max(t => t.Id);
            }
            if (users is not null)
            {
                _users = users;
                _lastUserId = _users.Max(u => u.Id);
            }
            Name = String.Empty;
        }
        /// <summary>
        /// Добавление нового пользователя
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        public User AddUser(string login, string password, UserType type)
        {
            if (_users.Any(u => u.Login == login)) throw new Exception("Пользователь с данным логином уже существует");
            _lastUserId++;
            var user = new User() { Login = login, Password = password, Type = type, Id = _lastUserId };
            _users.Add(user);
            return user;
        }

        public void RemoveUser(long id)
        {
            User user = _users.FirstOrDefault(u => u.Id == id);
            if (user is null) throw new Exception("Пользователя не существует");
            _users.Remove(user);
        }
        /// <summary>
        /// Добавить физ клиента
        /// </summary>
        /// <param name="firstName">имя</param>
        /// <param name="lastName">фамилия</param>
        public void AddIndividual(User user, string firstName, string lastName)
        {
            _lastClientId++;
            _clients.Add(new Individual
            {
                Id = _lastClientId,
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
            _lastClientId++;
            if (inn.Length != 10 || inn.Length != 12) throw new FormatException("ИНН должен содержать 10 или 12 символов");
            if (!Int64.TryParse(inn, out var longInn)) throw new FormatException("ИНН должен состоять только из цифр");

            _clients.Add(new Company
            {
                Id = _lastClientId,
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

            _lastAccountId++;
            var account = new Account
            {
                Id = _lastAccountId,
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

            _lastTransactionId++;
            var transaction = new Transaction
            {
                Id = _lastTransactionId,
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
