﻿using BankSystemPrototype.ApplicationServices.Helper;
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
        public async Task<Bank> GetBank(int id)
        {
            Bank bank = await Banks.FirstOrDefaultAsync(b => b.Id == id);
            if (bank is null) throw new Exception("Банк не найден");
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
        public async Task<Bank> AddEmptyBank(string bankName)
        {
            _lastBankId++;
            var bank = new Bank() { Name = bankName, Id = _lastBankId};
            await Banks.AddAsync(bank);
            return bank;
        }
        /// <summary>
        /// Удалить банк
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task RemoveBank(int id)
        {
            var bank = await Banks.FirstOrDefaultAsync(b => b.Id == id);
            if (bank is null) throw new Exception("Банка не существует");
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
        }
        /// <summary>
        /// Добавление нового пользователя
        /// </summary>
        /// <param name="bankId"></param>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<User> AddUser(int bankId, string login, string password, UserType type)
        {
            var bank = await Banks.FirstOrDefaultAsync(b => b.Id == bankId);
            if (bank is null) throw new Exception("Банк не найден");
            var user = bank.AddUser(login, password, type);
            await Users.AddAsync(user);
            return user;
        }
        /// <summary>
        /// Удаление пользователя
        /// </summary>
        /// <param name="bankId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<User> RemoveUser(int bankId, int userId)
        {
            var bank = await Banks.FirstOrDefaultAsync(b => b.Id == bankId);
            if (bank is null) throw new Exception("Банк не найден");
            var user = await Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user is null) throw new Exception("Пользователь не найден");
            bank.RemoveUser(user.Id);
            Users.Remove(user);
            return user;
        }
    }
}
