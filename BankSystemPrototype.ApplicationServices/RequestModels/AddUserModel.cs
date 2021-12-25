using BankSystemPrototype.Domain.UserModel;

namespace BankSystemPrototype.ApplicationServices.RequestModels
{
    public class AddUserModel
    {
        public long UserId { get; set; }
        public long BankId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public UserType Type { get; set; }
        public AddUserModel(long userId, long bankId, string login, string password, UserType type)
        {
            UserId = userId;
            BankId = bankId;
            Login = login;
            Password = password;
            Type = type;
        }
    }
    
}
