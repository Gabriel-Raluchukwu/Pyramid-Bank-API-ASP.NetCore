using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankTwoAPI_Data.Interfaces;
using BankTwoAPI_Entities.Models;
using BankTwoAPI_Entities.ViewModels;
using BankTwoCoreAPI.UtilityLogic;
using CoreInfrastructure;
using FluentValidation.Results;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BankTwoCoreAPI.Controllers
{
    [Route("api/aurora/[controller]")]
    [ApiController]
    public class MobileTopUpController : ControllerBase
    {
        public IAirtimeTopUpRepo<AirTimeTopUp> _airtimeTopUpRepo { get; }
        public ICustomerAccountRepo<CustomerAccount> _customerAccountRepo { get; }

        private ILogger _logger { get;}

        //Constructor
        public MobileTopUpController(IAirtimeTopUpRepo<AirTimeTopUp> airtimeTopUpRepo,ICustomerAccountRepo<CustomerAccount> customerAccountRepo,
            ILogger<MobileTopUpController> logger)
        {
            _airtimeTopUpRepo = airtimeTopUpRepo;
            _customerAccountRepo = customerAccountRepo;
            _logger = logger;
        }

        // GET: api/MobileTopUp
        [HttpGet]
        [EnableQuery()]
        public ActionResult<IEnumerable<MobileTopUpViewModel>> Get([FromQuery]int offset, [FromQuery] int count)
        {
            List<MobileTopUpViewModel> mobileTopUpViewModels = new List<MobileTopUpViewModel>();

            var mobileTopUpRecords = _airtimeTopUpRepo.GetAll(offset,count);
            foreach (var mobileTopUp in mobileTopUpRecords)
            {
                mobileTopUpViewModels.Add
                    (
                        new MobileTopUpViewModel
                        {
                            Id = mobileTopUp.Id,
                            CreatedAt = UnixDateTime.ConvertToUnixTime(mobileTopUp.CreatedAt),
                            MobileNetworkId = mobileTopUp.MobileNetworksId,
                            Amount = mobileTopUp.Amount,
                            CustomerAccountNumber = mobileTopUp.CustomerAccountNumber,
                        }
                    );
            }

            return mobileTopUpViewModels;
        }

        // GET: api/MobileTopUp/5
        [HttpGet("get/{id}")]
        [EnableQuery()]
        public ActionResult<MobileTopUpViewModel> Get(int id)
        {
            var topUpRecord = _airtimeTopUpRepo.Get(id);
            if (topUpRecord != null)
            {
                MobileTopUpViewModel mobileTopUpViewModel = new MobileTopUpViewModel
                {
                    Id = topUpRecord.Id,
                    CreatedAt = UnixDateTime.ConvertToUnixTime(topUpRecord.CreatedAt),
                    MobileNetworkId = topUpRecord.MobileNetworksId,
                    Amount = topUpRecord.Amount,
                    CustomerAccountNumber = topUpRecord.CustomerAccountNumber,
                };
                return mobileTopUpViewModel;
            }
            return NotFound();
        }

        [HttpGet("{accountNumber}")]
        [EnableQuery()]
        public ActionResult<IEnumerable<MobileTopUpViewModel>> GetCustomerTopUp(string accountNumber, [FromQuery]int offset, [FromQuery] int count)
        {
            List<MobileTopUpViewModel> customerMobileRecharges = new List<MobileTopUpViewModel>();
            var mobileTopUpRecords = _airtimeTopUpRepo.GetCustomerAirtimeTopUp(accountNumber,offset,count);
            foreach (var mobileTopUp in mobileTopUpRecords)
            {
                customerMobileRecharges.Add(
                    new MobileTopUpViewModel
                    {
                        Id = mobileTopUp.Id,
                        CreatedAt = UnixDateTime.ConvertToUnixTime(mobileTopUp.CreatedAt),
                        MobileNetworkId = mobileTopUp.MobileNetworksId,
                        Amount = mobileTopUp.Amount,
                        CustomerAccountNumber = mobileTopUp.CustomerAccountNumber,
                    });
            }
            return customerMobileRecharges;
        }

        // POST: api/MobileTopUp
        [HttpPost]
        public ActionResult Post([FromBody] MobileTopUpViewModel mobileTopUpViewModel)
        {
            MobileTopUpValidator mobileTopUpValidator = new MobileTopUpValidator();
            ValidationResult result = mobileTopUpValidator.Validate(mobileTopUpViewModel);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }
            CustomerAccount customerAccount = _customerAccountRepo.GetAccountViaAccountNumber(mobileTopUpViewModel.CustomerAccountNumber);
            if (customerAccount == null)
            {
                return BadRequest(new {error = " Invalid Account Details" });
            }
            PinAuthentication pinAuthentication = new PinAuthentication(_customerAccountRepo);
            if (pinAuthentication.AuthenticateAccountPin(mobileTopUpViewModel.CustomerAccountNumber,mobileTopUpViewModel.TransactionPin))
            {
                FundTransfer fundTransfer = new FundTransfer(_customerAccountRepo);
                bool debitCheck = fundTransfer.DebitAccount(mobileTopUpViewModel.CustomerAccountNumber,mobileTopUpViewModel.Amount,out object error);
                if (!debitCheck)
                {
                    return BadRequest(error);
                }

                AirTimeTopUp airTimeTopUp = new AirTimeTopUp
                {
                    CreatedAt = UnixDateTime.ConvertToDateTime(mobileTopUpViewModel.CreatedAt),
                    Amount = mobileTopUpViewModel.Amount,
                    CustomerAccountNumber = mobileTopUpViewModel.CustomerAccountNumber,
                    CustomerAccountId = customerAccount.Id,
                    MobileNetworksId = mobileTopUpViewModel.MobileNetworkId,
                    MobileNumber = mobileTopUpViewModel.MobileNumber,
                };
                bool dataBaseOperation = _airtimeTopUpRepo.Post(airTimeTopUp);
                if (!dataBaseOperation)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
                return CreatedAtAction("get",mobileTopUpViewModel);
            }

            return BadRequest(new { error = " Invalid Transaction Pin" });
        }

        // PUT: api/MobileTopUp/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] string value)
        {
            return StatusCode(StatusCodes.Status501NotImplemented);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            return StatusCode(StatusCodes.Status501NotImplemented);
        }
    }
}
