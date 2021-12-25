namespace BankSystemPrototype.ApplicationServices.RequestModels
{
    public class AddMoneyModel
    {
        public long UserId { get; set; }
        public long BankId { get; set; }
        public long AccountId { get; set; }
        public decimal Money { get; set; }
        public AddMoneyModel(long userId, long bankId, long accountId, decimal money)
        {
            UserId = userId;
            BankId = bankId;
            AccountId = accountId;
            Money = money;
        }
    }
    
}
