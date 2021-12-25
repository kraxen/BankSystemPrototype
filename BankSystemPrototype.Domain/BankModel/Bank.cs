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
        /// Код безопасности при добавлении, удалении банка
        /// </summary>
        public string SequrityKey { get; init; }
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
            SequrityKey = String.Empty;
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
            if (users is not null)
            {
                _users = users;
                _lastUserId = _users.Max(u => u.Id);
            }
            Name = String.Empty;
            Users.Add(new User()
            {
                Id = _lastUserId,
                Login = "admin",
                Password = "admin",
                Type = UserType.Admin
            });
            _lastUserId++;
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
        public User AddUser(User _user, string login, string password, UserType type)
        {
            if (!_user.IsUserCanAddUser) throw new Exception("Данный пользователь не может добавлять новых пользователей");
            if (_users.Any(u => u.Login == login)) throw new Exception("Пользователь с данным логином уже существует");
            _lastUserId++;
            var user = new User() { Login = login, Password = password, Type = type, Id = _lastUserId };
            _users.Add(user);
            return user;
        }
        public void RemoveUser(User _user, long id)
        {
            if (!_user.IsUserCanRemoveUser) throw new Exception("Данный пользователь не может удалять пользователей");
            User user = _users.FirstOrDefault(u => u.Id == id);
            if (user is null) throw new Exception("Пользователя не существует");
            _users.Remove(user);
        }
        /// <summary>
        /// Добавить физ клиента
        /// </summary>
        /// <param name="firstName">имя</param>
        /// <param name="lastName">фамилия</param>
        public Individual AddIndividual(User user, string firstName, string lastName)
        {
            if (!user.IsUserCanAddClient) throw new Exception("Данный пользователь не может создавать клиентов");
            _lastClientId++;
            var individual = new Individual
            {
                Id = _lastClientId,
                FirstName = firstName,
                LastName = lastName
            };
            _clients.Add(individual);
            return individual;
        }
        /// <summary>
        /// Добавить компанию
        /// </summary>
        /// <param name="inn">ИНН компании</param>
        /// <param name="name">Наименование компании</param>
        public Company AddCompany(User user, string inn, string name)
        {
            if (!user.IsUserCanAddClient) throw new Exception("Данный пользователь не может создавать клиентов");
            if (inn.Length != 10 || inn.Length != 12) throw new FormatException("ИНН должен содержать 10 или 12 символов");
            if (!Int64.TryParse(inn, out var longInn)) throw new FormatException("ИНН должен состоять только из цифр");

            _lastClientId++;
            var company = new Company
            {
                Id = _lastClientId,
                INN = inn,
                Name = name
            };
            _clients.Add(company);
            return company;
        }
        public void RemoveClient(User user, long clientId)
        {
            if (!user.IsUserCanRemoveClient) throw new Exception("Данный пользователь не может удалять клиентов");
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
        public Account AddAccount(User user, long clientId, AccountType type, decimal money = 0)
        {
            if (!user.IsUserCanAddAccount) throw new Exception("Данный пользователь не может добавлять счета");
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
            return account;
        }
        /// <summary>
        /// Удаление счета
        /// </summary>
        /// <param name="accountId"></param>
        public void RemoveAccount(User user, long accountId)
        {
            if (!user.IsUserCanRemoveAccount) throw new Exception("Данный пользователь не может удалять счета");
            var account = _accounts.FirstOrDefault(a => a.Id == accountId);
            if (account is null) throw new Exception("Не удалось найти счет по Id при удалении счета");

            // Удаление счета у владельца 
            account.Owner.RemoveAccount(account);
            // Удаление счета из банка
            _accounts.Remove(account);
        }
        /// <summary>
        /// Проведение транзакции
        /// </summary>
        /// <param name="user"></param>
        /// <param name="senderAccountId"></param>
        /// <param name="resipientAccountId"></param>
        /// <param name="money"></param>
        /// <returns></returns>
        public Transaction AddTransaction(User user, long senderAccountId, long resipientAccountId, decimal money)
        {
            if (!user.IsCanDoTransaction) throw new Exception("Данный пользователь не может проводить транзакции");
            
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

            return transaction;
        }
        /// <summary>
        /// Вход пользователя в банк
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        public void UserAuhtorize(string login, string password)
        {
            var user = Users.FirstOrDefault(u => u.Login == login);
            if (user is null) throw new Exception("Некорректный логин");
            user.TryAuhtorize(login,password);
        }
        /// <summary>
        /// Выход пользователя из системы
        /// </summary>
        /// <param name="userId"></param>
        public void UserExit(long userId)
        {
            var user = Users.FirstOrDefault(u => u.Id == userId);
            if (user is null) throw new Exception("Некорректный логин");
            user.Exit();
        }
        /// <summary>
        /// Промотать один год
        /// </summary>
        public void OneYearLate()
        {
            foreach (var a in Accounts) a.OneYearLate();
        }
        /// <summary>
        /// Добавление денег на счет
        /// </summary>
        /// <param name="user"></param>
        /// <param name="accountId"></param>
        /// <param name="money"></param>
        public void AddMoney(User user, long accountId, decimal money)
        {
            if (!user.IsCanAddMoney) throw new Exception("Данный пользователь не может добавлять деньги на счет");
            var account = Accounts.FirstOrDefault(a => a.Id == accountId);

            account.AddMoney(money);
        }
        /// <summary>
        /// Удаление денег со счета
        /// </summary>
        /// <param name="user"></param>
        /// <param name="accountId"></param>
        /// <param name="money"></param>
        public void SubtractMoney(User user, long accountId, decimal money)
        {
            if (!user.IsCanSubtractMoney) throw new Exception("Данный пользователь не может снимать деньги со счета");
            var account = Accounts.FirstOrDefault(a => a.Id == accountId);

            if(!account.SubtractMoney(money)) throw new Exception("На счете меньше денег, чем требуется вычесть. Невозможно выполнить операцию.");
        }
    }
}
