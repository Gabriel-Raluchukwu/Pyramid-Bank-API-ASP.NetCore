using System;
using System.Collections.Generic;
using System.Linq;
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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BankTwoCoreAPI.Controllers
{
    [Route("api/aurora/[controller]")]
    public class InterBankTransferController : Controller
    {
        public ICustomerAccountRepo<CustomerAccount> _customerAccountRepo { get; }
        public IInterBankTransferRepo<InterBankTransfer> _InterBankTransferRepo { get; }

        private ILogger _logger { get; }

        //Constructor
        public InterBankTransferController(ICustomerAccountRepo<CustomerAccount> customerAccountRepo,
            IInterBankTransferRepo<InterBankTransfer> interBankTransferRepo, ILogger<InterBankTransferController> logger)
        {
            _customerAccountRepo = customerAccountRepo;
            _InterBankTransferRepo = interBankTransferRepo;
            _logger = logger;
        }

        // GET: api/<controller>
        [HttpGet]
        [EnableQuery()]
        public ActionResult<IEnumerable<InterBankTransferViewModel>> Get([FromQuery]int offset, [FromQuery] int count)
        {
            var transfers = _InterBankTransferRepo.GetAll(offset,count);

            List<InterBankTransferViewModel> interBankTransfers = new List<InterBankTransferViewModel>();
            foreach (var transfer in transfers)
            {
                interBankTransfers.Add(new InterBankTransferViewModel
                {
                    Id = transfer.Id,
                    BanksId = transfer.BanksId,
                    BankName = transfer.Banks.BankShortName,
                    CustomerAccountNumber = transfer.CustomerAccountNumber,
                    RecipientAccountNumber = transfer.RecipientAccountNumber,
                    RecipientAccountName = transfer.RecipientAccountName,
                    TransferAmount = transfer.TransferAmount,
                    Description = transfer.Description,
                    CreatedAt = UnixDateTime.ConvertToUnixTime(transfer.CreatedAt)
                });
            }
            return interBankTransfers;
        }

        // GET api/<controller>/5
        [HttpGet("get/{id:int}")]
        [EnableQuery()]
        public ActionResult<InterBankTransferViewModel> Get(int id)
        {
            var transfer = _InterBankTransferRepo.Get(id);
            if (transfer != null)
            {
                InterBankTransferViewModel transferViewModel = new InterBankTransferViewModel
                {
                    Id = transfer.Id,
                    BanksId = transfer.BanksId,
                    BankName = transfer.Banks.BankShortName,
                    CustomerAccountNumber = transfer.CustomerAccountNumber,
                    RecipientAccountNumber = transfer.RecipientAccountNumber,
                    RecipientAccountName = transfer.RecipientAccountName,
                    TransferAmount = transfer.TransferAmount,
                    Description = transfer.Description,
                    CreatedAt = UnixDateTime.ConvertToUnixTime(transfer.CreatedAt),
                };
                return transferViewModel;
            }
            return StatusCode(StatusCodes.Status404NotFound);
        }

        [HttpGet("{accountNumber}")]
        [EnableQuery()]
        //TODO: Test GetCustomerTransfers
        public ActionResult<IEnumerable<InterBankTransferViewModel>> GetCustomerTransfers(string accountNumber, [FromQuery]int offset, [FromQuery] int count)
        {
            List<InterBankTransferViewModel> customerTransfers = new List<InterBankTransferViewModel>();
            var Transfers = _InterBankTransferRepo.GetCustomerAccountTransferRecords(accountNumber,offset,count);
            foreach (var transfer in Transfers)
            {
                customerTransfers.Add(
                    new InterBankTransferViewModel
                    {
                        Id = transfer.Id,
                        BanksId = transfer.BanksId,
                        BankName = transfer.Banks.BankShortName,
                        CustomerAccountNumber = transfer.CustomerAccountNumber,
                        RecipientAccountNumber = transfer.RecipientAccountNumber,
                        RecipientAccountName = transfer.RecipientAccountName,
                        TransferAmount = transfer.TransferAmount,
                        Description = transfer.Description,
                        CreatedAt = UnixDateTime.ConvertToUnixTime(transfer.CreatedAt),
                    });
            }
            return customerTransfers;    
        }

        // POST api/<controller>
        [HttpPost]
        public ActionResult Post([FromBody]InterBankTransferViewModel interBankTransferViewModel)
        {
            InterBankTransferValidator interBankValidator = new InterBankTransferValidator();
            ValidationResult result = interBankValidator.Validate(interBankTransferViewModel);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }

            PinAuthentication AuthenticatePin = new PinAuthentication(_customerAccountRepo);
            if (AuthenticatePin.AuthenticateAccountPin(interBankTransferViewModel.CustomerAccountNumber, interBankTransferViewModel.TransactionPin))
            {
                FundTransfer fundTransfer = new FundTransfer(_customerAccountRepo);
                var CustomerAccount = _customerAccountRepo.GetAccountViaAccountNumber(interBankTransferViewModel.CustomerAccountNumber);
                var RecipientAccount = _customerAccountRepo.GetAccountViaAccountNumber(interBankTransferViewModel.RecipientAccountNumber);
                if (CustomerAccount == null || RecipientAccount == null)
                {
                    return BadRequest(new { error = "Invalid Account details" });
                }
                bool debitResult = fundTransfer.DebitAccount(interBankTransferViewModel.CustomerAccountNumber, interBankTransferViewModel.TransferAmount, out object error);

                //Credit Recipient Account
                bool creditResult = fundTransfer.CreditAccount(interBankTransferViewModel.RecipientAccountNumber,interBankTransferViewModel.TransferAmount);

                if (!debitResult || !creditResult)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,error);
                }
                bool dbPostOperation = _InterBankTransferRepo.Post(new InterBankTransfer
                {
                    CreatedAt = UnixDateTime.ConvertToDateTime(interBankTransferViewModel.CreatedAt),
                    CustomerAccountId = interBankTransferViewModel.Id,
                    CustomerAccountNumber = interBankTransferViewModel.CustomerAccountNumber,
                    BanksId = interBankTransferViewModel.BanksId,
                    RecipientAccountNumber = interBankTransferViewModel.RecipientAccountNumber,
                    RecipientAccountName = interBankTransferViewModel.RecipientAccountName,
                    TransferAmount = interBankTransferViewModel.TransferAmount,
                    Description = interBankTransferViewModel.Description
                });
                if (!dbPostOperation)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
                return Ok();
              
            }
            //Something went wrong
            return StatusCode(StatusCodes.Status400BadRequest);
        }

        [HttpPut("update/{id}")]
        public ActionResult Put(int id, [FromBody] string value)
        {
            return StatusCode(StatusCodes.Status501NotImplemented);
        }

        // DELETE api/<controller>/5
        [HttpDelete("delete/{id}")]
        public ActionResult Delete(int id)
        {
            return StatusCode(StatusCodes.Status501NotImplemented);
        }
    }
}
