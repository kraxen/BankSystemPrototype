using BankSystemPrototype.ApplicationServices.DataContex;
using BankSystemPrototype.ApplicationServices.RequestModels;
using BankSystemPrototype.Domain.AccountModel;
using BankSystemPrototype.Domain.BankModel;
using BankSystemPrototype.Domain.ClientModel;
using BankSystemPrototype.Domain.TransactionModel;
using BankSystemPrototype.Domain.UserModel;
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
        private readonly IBankSystemDataContext _db;

        public BankSystemController(ILogger<BankSystemController> logger, IBankSystemDataContext db)
        {
            _logger = logger;
            _db = db;
        }
        /// <summary>
        /// Метод добавления счета клиенту
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("AddAccount")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Account), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Account>> AddAccount([FromBody] AddAccountModel model)
        {
            return Ok(await _db.AddAccount(model.UserId, model.BankId, model.ClientId, model.Type, model.Money));
        }

        /// <summary>
        /// Добавить нового клиента компанию
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("AddCompany")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Company), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Company>> AddCompany([FromBody] AddCompanyModel model)
        {
            return Ok(await _db.AddCompany(model.UserId, model.BankId, model.Inn, model.Name));
        }

        /// <summary>
        /// Добавить пусой банк
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("AddEmptyBank")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Bank), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Bank>> AddEmptyBank([FromBody] AddEmptyBankModel model)
        {
            return Ok(await _db.AddEmptyBank(model.BankName, model.Key));
        }

        /// <summary>
        /// Добавить нового клиента физ. лицо
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("AddIndividual")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Individual), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Individual>> AddIndividual([FromBody] AddIndividualModel model)
        {
            return Ok(await _db.AddIndividual(model.UserId, model.BankId, model.FirstName, model.LastName));
        }

        /// <summary>
        /// Добавить деньги клиенту
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("AddMoney")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> AddMoney([FromBody] AddMoneyModel model)
        {
            await _db.AddMoney(model.UserId, model.BankId, model.AccountId, model.Money);
            return Ok();
        }

        /// <summary>
        /// Добавить новую транзакцию/перевод
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("AddTransaction")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(TransactionInfo), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<TransactionInfo>> AddTransaction([FromBody] AddTransactionModel model)
        {
            return Ok(await _db.AddTransaction(model.UserId, model.BankId, model.SenderAccountId, model.ResipientAccountId, model.Money));
        }

        /// <summary>
        /// Добавить нового пользователя системы
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("AddUser")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IReadOnlyCollection<User>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<User>> AddUser([FromBody] AddUserModel model)
        {
            return Ok(await _db.AddUser(model.UserId, model.BankId, model.Login, model.Password, model.Type));
        }

        /// <summary>
        /// Получить счета по id банка
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        [HttpGet("GetAccounts")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IReadOnlyCollection<Account>), 200)]
        [ProducesResponseType(400)]
        public ActionResult<IReadOnlyCollection<Account>> GetAccounts([FromQuery] long bankId)
        {
            return Ok(_db.GetAccounts(bankId));
        }

        /// <summary>
        /// Получить банк по id
        /// </summary>
        /// <param name="bankId">id банка</param>
        /// <returns></returns>
        [HttpGet("GetBank")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Bank), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Bank>> GetBank([FromQuery] long bankId)
        {
            return Ok(await _db.GetBank(bankId));
        }

        /// <summary>
        /// Получить список всех банков базы данных
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetBanks")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IReadOnlyCollection<Bank>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IReadOnlyCollection<Bank>>> GetBanks()
        {
            return Ok(await _db.GetBanks());
        }

        /// <summary>
        /// Получить список всех пользователей базы данных
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUsers")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IReadOnlyCollection<User>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IReadOnlyCollection<User>>> GetUsers([FromQuery] long bankId)
        {
            return Ok(await _db.GetUsers(bankId));
        }
        

        /// <summary>
        /// Получить клиентов по id банка
        /// </summary>
        /// <param name="bankId">id банка</param>
        /// <returns></returns>
        [HttpGet("GetClients")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IReadOnlyCollection<Client>), 200)]
        [ProducesResponseType(400)]
        public ActionResult<IReadOnlyCollection<Client>> GetClients([FromQuery] long bankId)
        {
            return Ok(_db.GetClients(bankId));
        }


        /// <summary>
        /// Получить все транзакции банка
        /// </summary>
        /// <param name="bankId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTransactions")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IReadOnlyCollection<Transaction>), 200)]
        [ProducesResponseType(400)]
        public ActionResult<IReadOnlyCollection<Transaction>> GetTransactions([FromQuery] long bankId)
        {
            return Ok(_db.GetTransactions(bankId));
        }

        /// <summary>
        /// Промотать время банка на год вперед
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("OneYearLate")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> OneYearLate([FromQuery] long userId, [FromQuery] long bankId)
        {
            await _db.OneYearLate(userId, bankId);
            return Ok();
        }

        /// <summary>
        /// Удалить счет
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="bankId"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        [HttpPost("RemoveAccount")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> RemoveAccount([FromQuery] long userId, [FromQuery] long bankId, [FromQuery] long accountId)
        {
            await _db.RemoveAccount(userId, bankId, accountId);
            return Ok();
        }

        /// <summary>
        /// Удалить банк по Id
        /// </summary>
        /// <param name="bankId">Id банка</param>
        /// <param name="key">Ключ проверки безопасности</param>
        /// <returns></returns>
        [HttpPost("RemoveBank")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> RemoveBank([FromQuery] long bankId, [FromQuery] string key)
        {
            await _db.RemoveBank(bankId, key);
            return Ok();
        }


        /// <summary>
        /// Удалить клиента
        /// </summary>
        /// <param name="bankId"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpPost("RemoveClient")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> RemoveClient([FromQuery] long userId, [FromQuery] long bankId, [FromQuery] long clientId)
        {
            await _db.RemoveClient(userId, bankId, clientId);
            return Ok();
        }

        /// <summary>
        /// Удалить клиента
        /// </summary>
        /// <param name="bankId"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpPost("RemoveUser")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<User>> RemoveUser([FromQuery] long creatorId, [FromQuery] long bankId, [FromQuery] long userId)
        {
            return Ok(await _db.RemoveUser(creatorId, bankId, userId));
        }

        /// <summary>
        /// Снять деньги со счета
        /// </summary>
        [HttpPost("SubtractMoney")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> SubtractMoney([FromBody] SubtractMoneyModel model)
        {
            await _db.SubtractMoney(model.UserId, model.BankId, model.AccountId, model .Money);
            return Ok();
    }

        /// <summary>
        /// Авторизация пользователя в системе
        /// </summary>
        [HttpPost("UserAuhtorize")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400 )]
        public async Task<ActionResult> UserAuhtorize([FromBody] UserAuhtorizeModel model)
        {
            await _db.UserAuhtorize(model.BankId, model.Login, model.Password);
            return Ok();
        }

        [HttpPost("UserExit")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> UserExit([FromQuery] long bankId, [FromQuery] long userId)
        {
            await _db.UserExit(bankId, userId);
            return Ok();
        }
    }
}
