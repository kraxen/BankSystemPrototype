using BankSystemPrototype.ApplicationServices.Helper;
using BankSystemPrototype.Domain.AccountModel;
using BankSystemPrototype.Domain.BankModel;
using BankSystemPrototype.Domain.ClientModel;
using BankSystemPrototype.Domain.TransactionModel;
using BankSystemPrototype.Domain.UserModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankSystemPrototype.ApplicationServices.DataContex
{
    public class BankSystemEF : DbContext, IBankSystemDataContext
    {
        public DbSet<Bank> Banks { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Individual> Individuals { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionInfo> TransactionInfos { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=helloappdb;Trusted_Connection=True;");
        }
        /// <summary>
        /// Получить банк
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Bank> GetBank(long id)
        {
            Bank bank = await Banks.FirstOrDefaultAsync(b => b.Id == id);
            if (bank is null) throw new Exception("Банк не найден в базе данных");
            return bank;
        }
        /// <summary>
        /// Получить список банков
        /// </summary>
        /// <returns></returns>
        public async Task<IReadOnlyCollection<Bank>> GetBanks() => await Banks.ToListAsync();
        /// <summary>
        /// Получить список клиентов
        /// </summary>
        /// <returns></returns>
        public async Task<IReadOnlyCollection<Client>> GetClients(long bankId)
        {
            var bank = await Banks.FirstOrDefaultAsync(b => b.Id == bankId);
            if (bank is null) throw new Exception("Банк не найден в базе данных");

            return bank.Clients;
        }
        /// <summary>
        /// Получить список счетов
        /// </summary>
        /// <returns></returns>
        public async Task<IReadOnlyCollection<Account>> GetAccounts(long bankId)
        {
            var bank = await Banks.FirstOrDefaultAsync(b => b.Id == bankId);
            if (bank is null) throw new Exception("Банк не найден в базе данных");

            return bank.Accounts;
        }
        /// <summary>
        /// Получить список транзакций банка
        /// </summary>
        /// <param name="bankId"></param>
        /// <returns></returns>
        public async Task<IReadOnlyCollection<Transaction>> GetTransactions(long bankId)
        {
            var bank = await Banks.FirstOrDefaultAsync(b => b.Id == bankId);
            if (bank is null) throw new Exception("Банк не найден в базе данных");

            return bank.Transactions;
        }
        /// <summary>
        /// Получить список пользователей банка
        /// </summary>
        /// <param name="bankId"></param>
        /// <returns></returns>
        public async Task<IReadOnlyCollection<User>> GetUsers(long bankId)
        {
            var bank = await Banks.FirstOrDefaultAsync(b => b.Id == bankId);
            if (bank is null) throw new Exception("Банк не найден в базе данных");

            return bank.Users;
        }
        /// <summary>
        /// Добавить новый пустой банк
        /// </summary>
        /// <returns></returns>
        public async Task<Bank> AddEmptyBank(string bankName, string key)
        {
            if (await Banks.AnyAsync(b => b.Name == bankName)) throw new Exception("Банк с таким именем уже существует");
            var bank = new Bank() { Name = bankName, SequrityKey = key };
            await Banks.AddAsync(bank);
            await SaveChangesAsync();
            return bank;
        }
        /// <summary>
        /// Удалить банк
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task RemoveBank(long id, string key)
        {
            var bank = await Banks.FirstOrDefaultAsync(b => b.Id == id);
            if (bank is null) throw new Exception("Банка не существует в базе данных");
            if (bank.SequrityKey != key) throw new Exception("Код безопасности не подходит, вы не можете удалить банк в базе данных");
            Banks.Remove(bank);
            Users.RemoveRange(Users.Where(u => u.Bank.Id == bank.Id));
            Individuals.RemoveRange(Individuals.Where(i => i.Bank.Id == bank.Id));
            Companies.RemoveRange(Companies.Where(c => c.Bank.Id == bank.Id));
            Accounts.RemoveRange(Accounts.Where(a => a.Owner.Bank.Id == bank.Id));
            Transactions.RemoveRange(Transactions.Where(t=>t.Bank.Id == bank.Id));
            await SaveChangesAsync();
        }
        /// <summary>
        /// Добавление нового пользователя
        /// </summary>
        /// <param name="bankId"></param>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<User> AddUser(long userId, long bankId, string login, string password, UserType type)
        {
            var bank = await Banks.FirstOrDefaultAsync(b => b.Id == bankId);
            if (bank is null) throw new Exception("Банк не найден в базе данных");
            var creator = await Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (creator is null) throw new Exception("Пользователь не найден в базе данных");
            var user = bank.AddUser(creator, login, password, type);
            await Users.AddAsync(user);
            await SaveChangesAsync();
            return user;
        }
        /// <summary>
        /// Удаление пользователя
        /// </summary>
        /// <param name="bankId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<User> RemoveUser(long creatorId, long bankId, long userId)
        {
            var bank = await Banks.FirstOrDefaultAsync(b => b.Id == bankId);
            if (bank is null) throw new Exception("Банк не найден в базе данных");
            var user = await Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user is null) throw new Exception("Пользователь, которого нужно удалить, не найден в базе данных");
            var creator = await Users.FirstOrDefaultAsync(u => u.Id == creatorId);
            if (creator is null) throw new Exception("Пользователь, который производит удаление, не найден в базе данных");
            bank.RemoveUser(creator, user.Id);
            Users.Remove(user);
            await SaveChangesAsync();
            return user;
        }
        /// <summary>
        /// Авторизация пользователя в системе
        /// </summary>
        /// <param name="bankId"></param>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task UserAuhtorize(long bankId, string login, string password)
        {
            var bank = await Banks.FirstOrDefaultAsync(b => b.Id == bankId);
            if (bank is null) throw new Exception("Банк не найден в базе данных");

            var user = await Users.FirstOrDefaultAsync(u => u.Login == login);
            if (user is null) throw new Exception("Пользователь не найден в базе данных");

            user.TryAuhtorize(login, password);
            bank.UserAuhtorize(login, password);
            await SaveChangesAsync();
        }
        /// <summary>
        /// Вызод пользователя из системы
        /// </summary>
        /// <param name="bankId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task UserExit(long bankId, long userId)
        {
            var bank = await Banks.FirstOrDefaultAsync(b => b.Id == bankId);
            if (bank is null) throw new Exception("Банк не найден в базе данных");

            var user = await Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user is null) throw new Exception("Пользователь не найден в базе данных");

            user.Exit();
            bank.UserExit(userId);
            await SaveChangesAsync();
        }
        /// <summary>
        /// Добавление компании
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="bankId"></param>
        /// <param name="inn"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<Company> AddCompany(long userId, long bankId, string inn, string name)
        {
            var user = await Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user is null) throw new Exception("Пользователь не найден в базе данных");

            var bank = await Banks.FirstOrDefaultAsync(b => b.Id == bankId);
            if (bank is null) throw new Exception("Банк не найден в базе данных");

            var company = bank.AddCompany(user, inn, name);
            Companies.Add(company);
            await SaveChangesAsync();
            return company;
        }
        /// <summary>
        /// Добавление индивидуального клиента
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="bankId"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <returns></returns>
        public async Task<Individual> AddIndividual(long userId, long bankId, string firstName, string lastName)
        {
            var user = await Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user is null) throw new Exception("Пользователь не найден в базе данных");

            var bank = await Banks.FirstOrDefaultAsync(b => b.Id == bankId);
            if (bank is null) throw new Exception("Банк не найден в базе данных");

            var individual = bank.AddIndividual(user, firstName, lastName);
            await Individuals.AddAsync(individual);
            await SaveChangesAsync();
            return individual;
        }
        /// <summary>
        /// Удалить клиента
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="bankId"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public async Task RemoveClient(long userId, long bankId, long clientId)
        {
            var user = await Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user is null) throw new Exception("Пользователь не найден в базе данных");

            var bank = await Banks.FirstOrDefaultAsync(b => b.Id == bankId);
            if (bank is null) throw new Exception("Банк не найден в базе данных");

            bank.RemoveClient(user, clientId);

            Client client = await Individuals.FirstOrDefaultAsync(c => c.Id == clientId);
            if (client is not null) Individuals.Remove((Individual)client);
            else client = await Companies.FirstOrDefaultAsync(c => c.Id == clientId);
            if (client is null) throw new Exception("Клиент не найден в базе данных");
            else Companies.Remove((Company)client);
            await SaveChangesAsync();
        }
        /// <summary>
        /// Добавление счета клиенту
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="bankId"></param>
        /// <param name="clientId"></param>
        /// <param name="type"></param>
        /// <param name="money"></param>
        /// <returns></returns>
        public async Task<Account> AddAccount(long userId, long bankId, long clientId, AccountType type, decimal money = 0)
        {
            var user = await Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user is null) throw new Exception("Пользователь не найден в базе данных");

            var bank = await Banks.FirstOrDefaultAsync(b => b.Id == bankId);
            if (bank is null) throw new Exception("Банк не найден в базе данных");

            var account = bank.AddAccount(user, clientId, type, money);
            await Accounts.AddAsync(account);

            Client client = await Individuals.FirstOrDefaultAsync(c => c.Id == clientId);
            if (client is null) client = await Companies.FirstOrDefaultAsync(c => c.Id == clientId);
            if (client is null) throw new Exception("Клиент не найден в базе данных");

            client.AddAccount(account);
            await SaveChangesAsync();
            return account;
        }
        /// <summary>
        /// Добавление транзакции
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="bankId"></param>
        /// <param name="senderAccountId"></param>
        /// <param name="resipientAccountId"></param>
        /// <param name="money"></param>
        /// <returns></returns>
        public async Task<TransactionInfo> AddTransaction(long userId, long bankId, long senderAccountId, long resipientAccountId, decimal money)
        {
            var user = await Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user is null) throw new Exception("Пользователь не найден в базе данных");

            var bank = await Banks.FirstOrDefaultAsync(b => b.Id == bankId);
            if (bank is null) throw new Exception("Банк не найден в базе данных");

            var transaction = bank.AddTransaction(user, senderAccountId, resipientAccountId, money);
            await Transactions.AddAsync(transaction);
            await TransactionInfos.AddAsync(transaction.GetInfo());

            await SaveChangesAsync();
            return transaction.GetInfo();
        }
        /// <summary>
        /// Промотать один год
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="bankId"></param>
        /// <returns></returns>
        public async Task OneYearLate(long userId, long bankId)
        {
            var user = await Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user is null) throw new Exception("Пользователь не найден в базе данных");

            var bank = await Banks.FirstOrDefaultAsync(b => b.Id == bankId);
            if (bank is null) throw new Exception("Банк не найден в базе данных");

            bank.OneYearLate();
        }
        /// <summary>
        /// Добавить деньги на счет
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="bankId"></param>
        /// <param name="accountId"></param>
        /// <param name="money"></param>
        /// <returns></returns>
        public async Task AddMoney(long userId, long bankId, long accountId, decimal money)
        {
            var user = await Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user is null) throw new Exception("Пользователь не найден в базе данных");

            var bank = await Banks.FirstOrDefaultAsync(b => b.Id == bankId);
            if (bank is null) throw new Exception("Банк не найден в базе данных");

            var account = await Accounts.FirstOrDefaultAsync(a => a.Id == accountId);
            if (bank is null) throw new Exception("Счет не найден в базе данных");

            account.AddMoney(money);
            bank.AddMoney(user, accountId, money);
            await SaveChangesAsync();
        }
        /// <summary>
        /// Удаление счета
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="bankId"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task RemoveAccount(long userId, long bankId, long accountId)
        {
            var user = await Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user is null) throw new Exception("Пользователь не найден в базе данных");

            var bank = await Banks.FirstOrDefaultAsync(b => b.Id == bankId);
            if (bank is null) throw new Exception("Банк не найден в базе данных");

            var account = await Accounts.FirstOrDefaultAsync(a => a.Id == accountId);
            if (bank is null) throw new Exception("Счет не найден в базе данных");

            bank.RemoveAccount(user, accountId);
            Accounts.Remove(account);

            Client client = await Individuals.FirstOrDefaultAsync(c => c.GetAccounts.Contains(account));
            if (client is null) client = await Companies.FirstOrDefaultAsync(c => c.GetAccounts.Contains(account));
            if (client is null) throw new Exception("Клиент не найден в базе данных");
            else client.RemoveAccount(account);

            await SaveChangesAsync();
        }
        /// <summary>
        /// Удалить деньги со счета
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="bankId"></param>
        /// <param name="accountId"></param>
        /// <param name="money"></param>
        /// <returns></returns>
        public async Task SubtractMoney(long userId, long bankId, long accountId, decimal money)
        {
            var user = await Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user is null) throw new Exception("Пользователь не найден в базе данных");

            var bank = await Banks.FirstOrDefaultAsync(b => b.Id == bankId);
            if (bank is null) throw new Exception("Банк не найден в базе данных");

            var account = await Accounts.FirstOrDefaultAsync(a => a.Id == accountId);
            if (bank is null) throw new Exception("Счет не найден в базе данных");

            if (!account.SubtractMoney(money)) throw new Exception("На счете меньше денег, чем требуется вычесть. Невозможно выполнить операцию.");
            bank.SubtractMoney(user, accountId, money);
            await SaveChangesAsync();
        }
    }
}
