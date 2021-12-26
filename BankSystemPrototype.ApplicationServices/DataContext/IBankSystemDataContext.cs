using BankSystemPrototype.Domain.AccountModel;
using BankSystemPrototype.Domain.BankModel;
using BankSystemPrototype.Domain.ClientModel;
using BankSystemPrototype.Domain.TransactionModel;
using BankSystemPrototype.Domain.UserModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankSystemPrototype.ApplicationServices.DataContex
{
    public interface IBankSystemDataContext
    {
        Task<Account> AddAccount(long userId, long bankId, long clientId, AccountType type, decimal money = 0);
        Task<Company> AddCompany(long userId, long bankId, string inn, string name);
        Task<Bank> AddEmptyBank(string bankName, string key);
        Task<Individual> AddIndividual(long userId, long bankId, string firstName, string lastName);
        Task AddMoney(long userId, long bankId, long accountId, decimal money);
        Task<TransactionInfo> AddTransaction(long userId, long bankId, long senderAccountId, long resipientAccountId, decimal money);
        Task<User> AddUser(long userId, long bankId, string login, string password, UserType type);
        Task<IReadOnlyCollection<Account>> GetAccounts(long bankId);
        Task<Bank> GetBank(long id);
        Task<IReadOnlyCollection<Bank>> GetBanks();
        Task<IReadOnlyCollection<User>> GetUsers(long bankId);
        Task<IReadOnlyCollection<Client>> GetClients(long bankId);
        Task<IReadOnlyCollection<Transaction>> GetTransactions(long bankId);
        Task OneYearLate(long userId, long bankId);
        Task RemoveAccount(long userId, long bankId, long accountId);
        Task RemoveBank(long id, string key);
        Task RemoveClient(long userId, long bankId, long clientId);
        Task<User> RemoveUser(long creatorId, long bankId, long userId);
        Task SubtractMoney(long userId, long bankId, long accountId, decimal money);
        Task UserAuhtorize(long bankId, string login, string password);
        Task UserExit(long bankId, long userId);
    }
}