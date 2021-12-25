namespace BankSystemPrototype.ApplicationServices.RequestModels
{
    public class SubtractMoneyModel
    {
        public long UserId { get; set; }
        public long BankId { get; set; }
        public long AccountId { get; set; }
        public decimal Money { get; set; }
        public SubtractMoneyModel(long userId, long bankId, long accountId, decimal money)
        {
            UserId = userId;
            BankId = bankId;
            AccountId = accountId;
            Money = money;
        }
    }
    
}
