namespace BankSystemPrototype.ApplicationServices.RequestModels
{
    public class AddEmptyBankModel
    {
        public string BankName { get; set; }
        public string Key { get; set; }
        public AddEmptyBankModel(string bankName, string key)
        {
            BankName = bankName;
            Key = key;
        }
    }
    
}
