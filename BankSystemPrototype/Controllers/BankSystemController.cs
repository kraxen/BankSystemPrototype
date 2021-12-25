using BankSystemPrototype.ApplicationServices.RequestModels;
using BankSystemPrototype.Domain.AccountModel;
using BankSystemPrototype.Domain.BankModel;
using BankSystemPrototype.Domain.ClientModel;
using BankSystemPrototype.Domain.TransactionModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankSystemPrototype.Controllers
{
    [ApiController]
    [Route("v1/api/bank")]
    public class BankSystemController : ControllerBase
    {
        private readonly ILogger<BankSystemController> _logger;
        private readonly Bank _bank;

        public BankSystemController(ILogger<BankSystemController> logger, Bank bank)
        {
            _logger = logger;
            _bank = bank;
        }

        [HttpGet]
        [Route("GetClients")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IReadOnlyCollection<Client>), 200)]
        public ActionResult<IReadOnlyCollection<Client>> GetClients()
        {
            return Ok(_bank.Clients);
        }

        [HttpGet]
        [Route("GetAccounts")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IReadOnlyCollection<Account>), 200)]
        [ProducesResponseType(400)]
        public ActionResult<IReadOnlyCollection<Account>> GetAccounts(long clientId)
        {
            var client = _bank.Clients.FirstOrDefault(c => c.Id == clientId);
            if (client is null) return BadRequest("Не удалось найти клиента по данному Id");

            return Ok(client.GetAccounts);
        }

        [HttpGet]
        [Route("GetTransactions")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IReadOnlyCollection<Transaction>), 200)]
        [ProducesResponseType(400)]
        public ActionResult<IReadOnlyCollection<Transaction>> GetTransactions()
        {
            return Ok(_bank.Transactions);
        }

        [HttpGet]
        [Route("AddIndividual")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        public IActionResult AddIndividual(NewIndividualModel model)
        {
            _bank.AddIndividual(model.FirstName, model.LastName);
            return Ok();
        }

        [HttpGet]
        [Route("AddCompany")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        public IActionResult AddCompany(NewCompanyModel model)
        {
            _bank.AddCompany(model.Inn, model.Name);
            return Ok();
        }

        [HttpGet]
        [Route("RemoveClient")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        public IActionResult RemoveClient(long clientId)
        {
            _bank.RemoveClient(clientId);
            return Ok();
        }

        [HttpGet]
        [Route("AddAccount")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        public IActionResult AddAccount(NewAccountModel model)
        {
            AccountType type;
            switch (model.Type)
            {
                case "Account": type = AccountType.Account; break;
                case "Deposit": type = AccountType.Deposit; break;
                case "Credit": type = AccountType.Credit; break;
                default: return BadRequest("Некорректно указан тип продукта");
            }
            _bank.AddAccount(model.ClientId, type, model.Money);
            return Ok();
        }

        [HttpGet]
        [Route("RemoveAccount")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        public IActionResult RemoveAccount(long accountId)
        {
            _bank.RemoveAccount(accountId);
            return Ok();
        }

        [HttpGet]
        [Route("RemoveAccount")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        public ActionResult<TransactionInfo> AddTransaction(NewTransaction model)
        {
            return Ok(_bank.AddTransaction(model.SenderAccountId, model.ResipientAccountId, model.Money));
        }
    }
}
