using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystemPrototype.ApplicationServices.RequestModels
{
    public class NewTransaction
    {
        public long SenderAccountId { get; set; }
        public long ResipientAccountId { get; set; }
        public decimal Money { get; set; }
    }
}
