namespace BankSystemPrototype.ApplicationServices.RequestModels
{
    public class AddCompanyModel
    {
        public long UserId { get; set; }
        public long BankId { get; set; }
        public string Inn { get; set; }
        public string Name { get; set; }
        public AddCompanyModel(long userId, long bankId, string inn, string name)
        {
            UserId = userId;
            BankId = bankId;
            Inn = inn;
            Name = name;
        }
    }
    
}
