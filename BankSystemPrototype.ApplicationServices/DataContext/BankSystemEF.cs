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
    public class BankSystemEF : DbContext
    {
        private long _lastBankId = 0;
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
        public async Task<List<Bank>> GetBanks()
        {
            return await Banks.ToListAsync();
        }
        /// <summary>
        /// Добавить новый пустой банк
        /// </summary>
        /// <returns></returns>
        public async Task<Bank> AddEmptyBank(string bankName, string key)
        {
            _lastBankId++;
            var bank = new Bank() { Name = bankName, Id = _lastBankId, SequrityKey = key};
            await Banks.AddAsync(bank);
            await Users.AddRangeAsync(bank.Users);
            SaveChangesAsync();
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
            Users.RemoveRange(bank.Users);
            foreach(var client in bank.Clients)
            {
                if (client.GetType() is Company) Companies.Remove(Companies.First(c => c.Id == client.Id));
                else if (client.GetType() is Individual) Individuals.Remove(Individuals.First(c => c.Id == client.Id));

                //if (client.GetType() is Company) Companies.Remove((Company)client);
                //else if (client.GetType() is Individual) Individuals.Remove((Individual)client);
            }
            Accounts.RemoveRange(bank.Accounts);
            Transactions.RemoveRange(bank.Transactions);
            SaveChangesAsync();
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
            SaveChangesAsync();
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
            SaveChangesAsync();
            return user;
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
            SaveChangesAsync();
            return company;
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
            SaveChangesAsync();
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
            SaveChangesAsync();
        }
    }
}
