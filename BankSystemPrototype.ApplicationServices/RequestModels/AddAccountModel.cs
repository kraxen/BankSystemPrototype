using BankSystemPrototype.Domain.AccountModel;

namespace BankSystemPrototype.ApplicationServices.RequestModels
{
    /// <summary>
    /// Модель для запроса добавления нового счета
    /// </summary>
    public class AddAccountModel
    {
        public long UserId { get; set; }
        public long BankId { get; set; }
        public long ClientId { get; set; }
        public AccountType Type { get; set; }
        public decimal Money { get; set; }
        public AddAccountModel(long userId, long bankId, long clientId, AccountType type, decimal money = 0)
        {
            UserId = userId;
            BankId = bankId;
            ClientId = clientId;
            Type = type;
            Money = money;
        }
    }
    
}
