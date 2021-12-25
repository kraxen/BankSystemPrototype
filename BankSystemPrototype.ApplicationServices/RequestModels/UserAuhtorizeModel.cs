namespace BankSystemPrototype.ApplicationServices.RequestModels
{
    public class UserAuhtorizeModel
    {
        public long BankId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public UserAuhtorizeModel(long bankId, string login, string password)
        {
            BankId = bankId;
            Login = login;
            Password = password;
        }
    }
    
}
