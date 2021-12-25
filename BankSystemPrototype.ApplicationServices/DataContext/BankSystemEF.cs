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
            var bank = new Bank() { Name = bankName };
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
    }
}
