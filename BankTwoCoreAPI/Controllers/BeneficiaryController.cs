using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankTwoAPI_Data.Interfaces;
using BankTwoAPI_Entities.Models;
using BankTwoAPI_Entities.ViewModels;
using BankTwoCoreAPI.UtilityLogic;
using FluentValidation.Results;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BankTwoCoreAPI.Controllers
{
    [Route("api/aurora/[controller]")]
    [ApiController]
    public class BeneficiaryController : ControllerBase
    {

        public ICustomerBeneficiaryRepo<CustomerBeneficiary> _customerBeneficiaryRepo { get; }

        private ILogger _logger { get; }

        //Constructor
        public BeneficiaryController(ICustomerBeneficiaryRepo<CustomerBeneficiary> customerBeneficiaryRepo,
            ILogger<BeneficiaryController> logger)
        {
            _customerBeneficiaryRepo = customerBeneficiaryRepo;
            _logger = logger;
        }

        // GET: api/Beneficiary
        [HttpGet]
        [EnableQuery()]
        public ActionResult<IEnumerable<BeneficiaryViewModel>> Get([FromQuery]int offset, [FromQuery] int count)
        {
            var beneficiaries = _customerBeneficiaryRepo.GetAll(offset,count);
            List<BeneficiaryViewModel> beneficiaryViewModels = new List<BeneficiaryViewModel>();

            foreach (var beneficiary in beneficiaries)
            {
                beneficiaryViewModels.Add(
                    new BeneficiaryViewModel
                    {
                        Id = beneficiary.Id,
                        CreatedAt = UnixDateTime.ConvertToUnixTime(beneficiary.CreatedAt),
                        LastUpdatedAt = UnixDateTime.ConvertToUnixTime(beneficiary.LastUpdatedAt),
                        CustomerId = beneficiary.CustomerId,
                        CustomerAccountNumber = beneficiary.CustomerAccountNumber,
                        BankId = beneficiary.BankId,
                        BankShortName = beneficiary.Banks.BankShortName,
                        RecipientAccountNumber = beneficiary.RecipientAccountNumber,
                        RecipientAccountName = beneficiary.RecipientAccountName,
                        NickName = beneficiary.NickName
                    });
            }
            return beneficiaryViewModels;
        }

        // GET: api/Beneficiary/5
        [HttpGet("get/{id}")]
        [EnableQuery()]
        public ActionResult<BeneficiaryViewModel> Get(int id)
        {
            var beneficiary = _customerBeneficiaryRepo.Get(id);
            if (beneficiary == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            BeneficiaryViewModel beneficiaryViewModel = new BeneficiaryViewModel
            {
                Id = beneficiary.Id,
                CreatedAt = UnixDateTime.ConvertToUnixTime(beneficiary.CreatedAt),
                LastUpdatedAt = UnixDateTime.ConvertToUnixTime(beneficiary.LastUpdatedAt),
                CustomerId = beneficiary.CustomerId,
                CustomerAccountNumber = beneficiary.CustomerAccountNumber,
                BankId = beneficiary.BankId,
                BankShortName = beneficiary.Banks.BankShortName,
                RecipientAccountNumber = beneficiary.RecipientAccountNumber,
                RecipientAccountName = beneficiary.RecipientAccountName,
                NickName = beneficiary.NickName
            };
            return beneficiaryViewModel;
        
        }

        [HttpGet("{accountNumber}")]
        [EnableQuery()]
        public ActionResult<IEnumerable<BeneficiaryViewModel>> GetCustomerBeneficiaries(string accountNumber,[FromQuery]int offset,[FromQuery] int count)
        {
            List<BeneficiaryViewModel> customerBeneficiaries = new List<BeneficiaryViewModel>();
            var beneficiaries = _customerBeneficiaryRepo.GetCustomerBeneficiaries(accountNumber,offset,count);
            foreach (var beneficiary in beneficiaries)
            {
                customerBeneficiaries.Add(
                    new BeneficiaryViewModel
                    {
                        Id = beneficiary.Id,
                        CreatedAt = UnixDateTime.ConvertToUnixTime(beneficiary.CreatedAt),
                        LastUpdatedAt = UnixDateTime.ConvertToUnixTime(beneficiary.LastUpdatedAt),
                        CustomerId = beneficiary.CustomerId,
                        CustomerAccountNumber = beneficiary.CustomerAccountNumber,
                        BankId = beneficiary.BankId,
                        BankShortName = beneficiary.Banks.BankShortName,
                        RecipientAccountNumber = beneficiary.RecipientAccountNumber,
                        RecipientAccountName = beneficiary.RecipientAccountName,
                        NickName = beneficiary.NickName
                    });
            }
            return customerBeneficiaries;
        }

        // POST: api/Beneficiary
        [HttpPost]
        public ActionResult Post([FromBody] BeneficiaryViewModel beneficiaryViewModel)
        {
            BeneficiaryValidator beneficiaryValidator = new BeneficiaryValidator();
            ValidationResult result = beneficiaryValidator.Validate(beneficiaryViewModel);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }
            // Validate OTP Code
            if (true)
            {
                CustomerBeneficiary beneficiary = new CustomerBeneficiary
                {
                    IsActive = true,
                    CreatedAt = UnixDateTime.ConvertToDateTime(beneficiaryViewModel.CreatedAt),
                    LastUpdatedAt = UnixDateTime.ConvertToDateTime(beneficiaryViewModel.LastUpdatedAt),
                    CustomerId = beneficiaryViewModel.CustomerId,
                    CustomerAccountNumber = beneficiaryViewModel.CustomerAccountNumber,
                    BankId = beneficiaryViewModel.BankId,
                    RecipientAccountName = beneficiaryViewModel.RecipientAccountName,
                    RecipientAccountNumber = beneficiaryViewModel.RecipientAccountNumber,
                    NickName = beneficiaryViewModel.NickName
                };
                bool dbOperationResult = _customerBeneficiaryRepo.Post(beneficiary);
                if (!dbOperationResult)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
                return CreatedAtAction("Get",beneficiaryViewModel);
            }
            return BadRequest(new {error = "Invalid OTP code" });
        }

        // PUT: api/Beneficiary/5
        [HttpPut("{id}")]
        public ActionResult Put(int id,BeneficiaryViewModel beneficiaryViewModel)
        {
            return StatusCode(StatusCodes.Status501NotImplemented);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
           bool dbOperationResult = _customerBeneficiaryRepo.Delete(id);
            if (!dbOperationResult)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok();
        }
    }
}
