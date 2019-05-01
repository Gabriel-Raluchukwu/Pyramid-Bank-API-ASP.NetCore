using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankTwoAPI_Data.Interfaces;
using BankTwoAPI_Entities.Models;
using BankTwoAPI_Entities.ViewModels;
using BankTwoCoreAPI.UtilityLogic;
using BankTwoCoreAPI.UtilityLogic.Email;
using FluentValidation.Results;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BankTwoCoreAPI.Controllers
{
    [Route("api/aurora/[controller]")]
    [ApiController]
    public class CustomerAccountController : ControllerBase
    {
        public ICustomerAccountRepo<CustomerAccount> _customerAccountRepo{ get; }

        private IEmailSender _emailSender;
        private ILogger _logger { get; }

        //Constructor
        public CustomerAccountController(ICustomerAccountRepo<CustomerAccount> customerAccountRepo,
            ILogger<CustomerAccountController> logger, IEmailSender emailSender)
        {
            _customerAccountRepo = customerAccountRepo;
            _emailSender = emailSender;
            _logger = logger;
        }
        // GET api/values
        [HttpGet]
        [EnableQuery()]
        public ActionResult<IEnumerable<CustomerAccountViewModel>> Get([FromQuery]int offset, [FromQuery] int count)
        {
            List<CustomerAccountViewModel> customerAccountViewModels = new List<CustomerAccountViewModel>();
            var AllCustomerAccounts = _customerAccountRepo.GetAll(offset,count);
            foreach (var customerAccount in AllCustomerAccounts)
            {
                customerAccountViewModels.Add(new CustomerAccountViewModel
                {
                    CreatedAt = UnixDateTime.ConvertToUnixTime(customerAccount.CreatedAt),
                    LastUpdated = UnixDateTime.ConvertToUnixTime(customerAccount.LastUpdatedAt),
                    Id = customerAccount.Id,
                    CustomerAccountNumber = customerAccount.CustomerAccountNumber,
                    Surname = customerAccount.Surname,
                    FirstName = customerAccount.FirstName,
                    OtherNames = customerAccount.OtherNames,
                    BVNNumber = customerAccount.BVNNumber,
                    PhoneNumber = customerAccount.PhoneNumber,
                    Email =customerAccount.Email,
                    AccountType = (int) customerAccount.AccountType,
                    AccountBalance = customerAccount.AccountBalance
                });
            }
            return customerAccountViewModels;
        }

        // GET api/values/5
        [HttpGet("get/{id}")]
        [EnableQuery()]
        public ActionResult<CustomerAccountViewModel> Get(int id)
        {
            var customerAccount = _customerAccountRepo.Get(id);
            if (customerAccount != null)
            {
                CustomerAccountViewModel customerAccountViewModel = new CustomerAccountViewModel
                {
                    CreatedAt = UnixDateTime.ConvertToUnixTime(customerAccount.CreatedAt),
                    LastUpdated = UnixDateTime.ConvertToUnixTime(customerAccount.LastUpdatedAt),
                    Id = customerAccount.Id,
                    CustomerAccountNumber = customerAccount.CustomerAccountNumber,
                    Surname = customerAccount.Surname,
                    FirstName = customerAccount.FirstName,
                    OtherNames = customerAccount.OtherNames,
                    BVNNumber = customerAccount.BVNNumber,
                    PhoneNumber = customerAccount.PhoneNumber,
                    Email = customerAccount.Email,
                    AccountType = (int)customerAccount.AccountType,
                    AccountBalance = customerAccount.AccountBalance
                };
                return customerAccountViewModel;
            }
            return NotFound();
        }

        // POST api/values
        [HttpPost]
        public ActionResult Post([FromBody] CustomerAccountViewModel customerAccountViewModel)
        {
            CustomerAccountValidator customerAccountValidator = new CustomerAccountValidator();
            ValidationResult result = customerAccountValidator.Validate(customerAccountViewModel);

            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }

            string AccountNumber = Generate.GenerateAccountNumber();
            while (_customerAccountRepo.GetAccountViaAccountNumber(AccountNumber) != null)
            {
                AccountNumber = Generate.GenerateAccountNumber();
            }
            CustomerAccount customerAccount = new CustomerAccount
            {
                CreatedAt = UnixDateTime.ConvertToDateTime(customerAccountViewModel.CreatedAt),
                LastUpdatedAt = UnixDateTime.ConvertToDateTime(customerAccountViewModel.CreatedAt),
                CustomerAccountNumber = AccountNumber,
                Surname = customerAccountViewModel.Surname,
                FirstName = customerAccountViewModel.FirstName,
                OtherNames = customerAccountViewModel.OtherNames,
                BVNNumber = customerAccountViewModel.BVNNumber,
                PhoneNumber = customerAccountViewModel.PhoneNumber,
                Email = customerAccountViewModel.Email,
                AccountType = (CustomerAccountType)customerAccountViewModel.AccountType, //Enum.Parse(CustomerAccountType,customerAccountViewModel.AccountType)
                TransactionPin = customerAccountViewModel.TransactionPin,
            };
            //TODO: Send Email with AccountNumber And Verification Code
            string name = customerAccount.Surname + customerAccount.FirstName + customerAccount.OtherNames;
            bool emailSent = _emailSender.SendEmail(name ,customerAccount.Email,customerAccount.CustomerAccountNumber,Generate.GenerateVerificationCode());
            if (emailSent)
            {
                bool dbOperationResult = _customerAccountRepo.Post(customerAccount);
                if (!dbOperationResult)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }

                return CreatedAtAction("Get", customerAccountViewModel);
            }
            return StatusCode(StatusCodes.Status503ServiceUnavailable);
        }

        // PUT api/values/5
        [HttpPut("update/{id}")]
        public ActionResult Put(int id, [FromBody] CustomerAccountViewModel customerAccountViewModel)
        {
            var customerAccount = _customerAccountRepo.Get(id);

            CustomerAccountValidator customerAccountValidator = new CustomerAccountValidator();
            ValidationResult result = customerAccountValidator.Validate(customerAccountViewModel);
            if (!result.IsValid)
            {
                return BadRequest(result.ToString("-"));
            }

            if (customerAccount != null)
            {
                customerAccount.LastUpdatedAt = UnixDateTime.ConvertToDateTime(customerAccountViewModel.LastUpdated);
                customerAccount.FirstName = customerAccountViewModel.FirstName;
                customerAccount.Surname = customerAccountViewModel.Surname;
                customerAccount.OtherNames = customerAccountViewModel.OtherNames;
                customerAccount.PhoneNumber = customerAccountViewModel.PhoneNumber;
                customerAccount.Email = customerAccountViewModel.Email;

                var dbOperationResult = _customerAccountRepo.Put(customerAccount);
                if (!dbOperationResult)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
                return Ok();
            }
            return NotFound();
        }
        [HttpPut("changePin/{id}")]
        public ActionResult Put(int id,[FromBody] CustomerTransactionPinViewModel transactionPinViewModel)
        {
            DateTime DateTimeUpdated = DateTime.Now;
            var customerAccount = _customerAccountRepo.Get(id);

            if (customerAccount != null)
            {
                if (customerAccount.TransactionPin == transactionPinViewModel.OldTransactionPin)
                {
                    customerAccount.TransactionPin = transactionPinViewModel.NewTransactionPin;
                    var dbOperationResult = _customerAccountRepo.Put(customerAccount);
                    if (!dbOperationResult)
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError);
                    }
                }
                return Ok();
            }
            return NotFound();
        }

        // DELETE api/values/5
        [HttpDelete("delete/{id}")]
        public ActionResult Delete(int id)
        {
            bool dbOperationResult = _customerAccountRepo.Delete(id);
            if (!dbOperationResult)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok();
        }
    }

}
