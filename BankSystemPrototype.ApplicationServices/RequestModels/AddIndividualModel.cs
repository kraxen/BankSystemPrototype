namespace BankSystemPrototype.ApplicationServices.RequestModels
{
    public class AddIndividualModel
    {
        public long UserId { get; set; }
        public long BankId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public AddIndividualModel(long userId, long bankId, string firstName, string lastName)
        {
            UserId = userId;
            BankId = bankId;
            FirstName = firstName;
            LastName = lastName;
        }
    }
    
}
