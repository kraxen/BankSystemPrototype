namespace BankSystemPrototype.ApplicationServices.RequestModels
{
    public class AddTransactionModel
    {
        public long UserId { get; set; }
        public long BankId { get; set; }
        public long SenderAccountId { get; set; }
        public long ResipientAccountId { get; set; }
        public decimal Money { get; set; }
        public AddTransactionModel(long userId, long bankId, long senderAccountId, long resipientAccountId, decimal money)
        {
            UserId = userId;
            BankId = bankId;
            SenderAccountId = senderAccountId;
            ResipientAccountId = resipientAccountId;
            Money = money;
        }
    }
    
}
