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
    public class IntraBankTransferController : ControllerBase
    {
        public ICustomerAccountRepo<CustomerAccount> _customerAccountRepo { get;}
        public IIntraBankTransferRepo<IntraBankTransfer> _IntraBankTransferRepo { get; }

        private ILogger _logger { get;}

        //Constructor
        public IntraBankTransferController(ICustomerAccountRepo<CustomerAccount> customerAccountRepo,
            IIntraBankTransferRepo<IntraBankTransfer> intraBankTransferRepo, ILogger<IntraBankTransferController> logger)
        {
            _customerAccountRepo = customerAccountRepo;
            _IntraBankTransferRepo = intraBankTransferRepo;
            _logger = logger;
        }

        //GET: api/IntraTransfer
        [HttpGet]
        [EnableQuery()]
        public ActionResult<IEnumerable<IntraBankTransferViewModel>> Get([FromQuery]int offset, [FromQuery] int count)
        {
            var transfers = _IntraBankTransferRepo.GetAll(offset,count);

            List<IntraBankTransferViewModel> intraBankTransferViewModels = new List<IntraBankTransferViewModel>();
            foreach (var transfer in transfers)
            {
                intraBankTransferViewModels.Add(
                    new IntraBankTransferViewModel
                    {
                        Id = transfer.Id,
                        CustomerAccountId = transfer.CustomerAccountId,
                        CustomerAccountNumber = transfer.CustomerAccountNumber,
                        RecipientBankId = transfer.RecipientBankId,
                        RecipientAccountName = transfer.RecipientAccountName,
                        RecipientAccountNumber = transfer.RecipientAccountNumber,
                        RecipientBankName = transfer.Banks.BankShortName,
                        TransferAmount = transfer.TransferAmount,
                        Description = transfer.Description,
                        CreatedAt = UnixDateTime.ConvertToUnixTime(transfer.CreatedAt),
                    });
            }
            return Ok(); ;
        }

        // GET: api/IntraTransfer/5
        [HttpGet("get/{id}")]
        [EnableQuery()]
        public ActionResult<IntraBankTransferViewModel> Get(int id)
        {
            var transfer = _IntraBankTransferRepo.Get(id);
            if (transfer != null)
            { 
                var transferViewModel = new IntraBankTransferViewModel
                {
                    Id = transfer.Id,
                    CustomerAccountId = transfer.CustomerAccountId,
                    CustomerAccountNumber = transfer.CustomerAccountNumber,
                    RecipientBankId = transfer.RecipientBankId,
                    RecipientAccountName = transfer.RecipientAccountName,
                    RecipientAccountNumber = transfer.RecipientAccountNumber,
                    RecipientBankName = transfer.Banks.BankShortName,
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
        public ActionResult<IEnumerable<IntraBankTransferViewModel>> GetCustomerTransfers([FromRoute]string accountNumber, [FromQuery]int offset, [FromQuery] int count)
        {
            List<IntraBankTransferViewModel> customerTransfers = new List<IntraBankTransferViewModel>();
            var Transfers = _IntraBankTransferRepo.GetCustomerAccountTransferRecords(accountNumber,offset,count);
            foreach (var transfer in Transfers)
            {
                customerTransfers.Add(
                    new IntraBankTransferViewModel
                    {
                        Id = transfer.Id,
                        CustomerAccountId = transfer.CustomerAccountId,
                        CustomerAccountNumber = transfer.CustomerAccountNumber,
                        RecipientBankId = transfer.RecipientBankId,
                        RecipientAccountName = transfer.RecipientAccountName,
                        RecipientAccountNumber = transfer.RecipientAccountNumber,
                        RecipientBankName = transfer.Banks.BankShortName,
                        TransferAmount = transfer.TransferAmount,
                        Description = transfer.Description,
                        CreatedAt = UnixDateTime.ConvertToUnixTime(transfer.CreatedAt),
                    });
            }
            return customerTransfers;
        }

        // POST: api/IntraTransfer
        [HttpPost]
        public ActionResult Post([FromBody] IntraBankTransferViewModel intraBankTransferViewModel)
        {
            IntraBankTransferValidator intraBankValidator = new IntraBankTransferValidator();
            ValidationResult result = intraBankValidator.Validate(intraBankTransferViewModel);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }
           
            PinAuthentication pinAuthentication = new PinAuthentication(_customerAccountRepo);
            if (pinAuthentication.AuthenticateAccountPin(intraBankTransferViewModel.CustomerAccountNumber,intraBankTransferViewModel.TransactionPin))
            {
                FundTransfer fundTransfer = new FundTransfer(_customerAccountRepo);
                var customerAccount = _customerAccountRepo.GetAccountViaAccountNumber(intraBankTransferViewModel.CustomerAccountNumber);
                if (customerAccount == null)
                {
                    return BadRequest(new { error = "Invalid Account Number" });
                }
                bool debitResult = fundTransfer.DebitAccount(intraBankTransferViewModel.CustomerAccountNumber,intraBankTransferViewModel.TransferAmount, out object error);
                //Make API call to Corresponding bank
                //Default
                bool creditResult = true;
                if (!debitResult || !creditResult)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,error);
                }
                bool dbPostOperation = _IntraBankTransferRepo.Post(new IntraBankTransfer
                {
                   CreatedAt = UnixDateTime.ConvertToDateTime(intraBankTransferViewModel.CreatedAt),
                   CustomerAccountId = intraBankTransferViewModel.CustomerAccountId,
                   CustomerAccountNumber = intraBankTransferViewModel.CustomerAccountNumber,
                   RecipientBankId = intraBankTransferViewModel.RecipientBankId,
                   RecipientAccountNumber = intraBankTransferViewModel.RecipientAccountNumber,
                   RecipientAccountName = intraBankTransferViewModel.RecipientAccountName,
                   RecipientBankName = intraBankTransferViewModel.RecipientBankName,
                   Description = intraBankTransferViewModel.Description,
                   TransferAmount = intraBankTransferViewModel.TransferAmount
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

        //public ActionResult Recieve([FromBody] InterBankTransferViewModel transferViewModel)
        //{


        //}

        // PUT: api/IntraTransfer/5
        [HttpPut("update/{id}")]
        public ActionResult Put(int id, [FromBody] string value)
        {
            return StatusCode(StatusCodes.Status501NotImplemented);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("delete/{id}")]
        public ActionResult Delete(int id)
        {
            return StatusCode(StatusCodes.Status501NotImplemented);
        }
    }
}
