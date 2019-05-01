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
    public class CardRequestController : ControllerBase
    {
        public ICardRequestRepo<CardRequest> _cardRequestRepo { get; }
        public ICustomerAccountRepo<CustomerAccount> _customerAccountRepo { get; }

        private ILogger _logger { get; }

        //Constructor
        public CardRequestController(ICardRequestRepo<CardRequest> cardRequestRepo,
           ICustomerAccountRepo<CustomerAccount> customerAccountRepo, ILogger<CardRequestController> logger)
        {
            _cardRequestRepo = cardRequestRepo;
            _customerAccountRepo = customerAccountRepo;
            _logger = logger;
        }

        // GET: api/CardRequest
        [HttpGet]
        [EnableQuery()]
        public ActionResult<IEnumerable<CardRequestViewModel>> Get([FromQuery]int offset, [FromQuery] int count)
        {
            List<CardRequestViewModel> cardRequestViewModels = new List<CardRequestViewModel>();
            var cardRequests = _cardRequestRepo.GetAll(offset,count);

            foreach (var request in cardRequests)
            {
                cardRequestViewModels.Add(
               new CardRequestViewModel
               {
                   Id = request.Id,
                   CreatedAt = UnixDateTime.ConvertToUnixTime(request.CreatedAt),
                   CustomerAccountId = request.CustomerAccountId,
                   CustomerAccountNumber = request.CustomerAccountNumber,
                   CardType = (int)request.CardType,
                   Description = request.Description

               });
            }
            return cardRequestViewModels;
        }

        // GET: api/CardRequest/5
        [HttpGet("get/{id}")]
        [EnableQuery()]
        public ActionResult<CardRequestViewModel> Get(int id)
        {
            var cardRequest = _cardRequestRepo.Get(id);
            if (cardRequest != null)
            {
                CardRequestViewModel cardRequestViewModel = new CardRequestViewModel
                {
                    Id = cardRequest.Id,
                    CreatedAt = UnixDateTime.ConvertToUnixTime(cardRequest.CreatedAt),
                    CustomerAccountId = cardRequest.CustomerAccountId,
                    CustomerAccountNumber = cardRequest.CustomerAccountNumber,
                    CardType = (int)cardRequest.CardType,
                    Description = cardRequest.Description
                };
                return cardRequestViewModel;
            }
            return NotFound();
        }

        [HttpGet("{accountNumber}")]
        [EnableQuery()]
        public ActionResult<IEnumerable<CardRequestViewModel>> GetCustomerCardRequests(string accountNumber, [FromQuery]int offset, [FromQuery] int count)
        {
            List<CardRequestViewModel> customerCardRequests = new List<CardRequestViewModel>();
            var cardRequests = _cardRequestRepo.GetCustomerCardRequests(accountNumber,offset,count);
            foreach (var request in cardRequests)
            {
                customerCardRequests.Add(
                    new CardRequestViewModel
                    {
                        Id = request.Id,
                        CreatedAt = UnixDateTime.ConvertToUnixTime(request.CreatedAt),
                        CustomerAccountId = request.CustomerAccountId,
                        CustomerAccountNumber = request.CustomerAccountNumber,
                        CardType = (int)request.CardType,
                        Description = request.Description
                    });
            }
            return customerCardRequests;
        }

        // POST: api/CardRequest
        [HttpPost]
        public ActionResult Post([FromBody] CardRequestViewModel requestViewModel)
        {
            CardRequestValidator cardRequestValidator = new CardRequestValidator();
            ValidationResult result = cardRequestValidator.Validate(requestViewModel);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }
            var customerAccount = _customerAccountRepo.GetAccountViaAccountNumber(requestViewModel.CustomerAccountNumber);
            if (customerAccount == null)
            {
                return BadRequest(new {error = "Invalid Account Details"});
            }
            PinAuthentication pinAuthentication = new PinAuthentication(_customerAccountRepo);
            if (pinAuthentication.AuthenticateAccountPin(requestViewModel.CustomerAccountNumber,requestViewModel.TransactionPin))
            {
                RequestCard cardRequestInfrastructure = new RequestCard(_customerAccountRepo);
                bool cardRequest = cardRequestInfrastructure.CardChargeDebit(customerAccount.CustomerAccountNumber,(CardType) requestViewModel.CardType);
                if (cardRequest)
                {
                    bool check = _cardRequestRepo.Post(
                        new CardRequest {
                            CreatedAt = UnixDateTime.ConvertToDateTime(requestViewModel.CreatedAt),
                            CustomerAccountId = requestViewModel.CustomerAccountId,
                            CustomerAccountNumber = requestViewModel.CustomerAccountNumber,
                            Description = requestViewModel.Description,
                            CardType = (CardType) requestViewModel.CardType
                        });
                    if (!check)
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError);
                    }
                    return CreatedAtAction("Get",requestViewModel);
                }
                //return BadRequest(new { error = "Insufficient Funds" });
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return BadRequest(new { error = " Invalid Transaction Pin" });
        }

        // PUT: api/CardRequest/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, CardRequestViewModel cardRequestViewModel)
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
