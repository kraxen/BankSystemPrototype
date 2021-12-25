namespace BankSystemPrototype.ApplicationServices.RequestModels
{
    public class NewAccountModel
    {
        public long ClientId { get; set; }
        public string Type { get; set; }
        public decimal Money { get; set; }
    }
}
