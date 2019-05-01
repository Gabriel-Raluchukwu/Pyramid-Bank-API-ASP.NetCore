using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankTwoAPI_Data;
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
    public class BankRegisterController : ControllerBase
    {
        public  IBankRepo<Banks> _bankRepo { get; }

        private ILogger _logger { get; }

        public BankRegisterController(IBankRepo<Banks> bankRepo,ILogger<BankRegisterController> logger)
        {
            _bankRepo = bankRepo;
            _logger = logger;
        }

        // GET api/values
        [HttpGet]
        [EnableQuery()]
        public ActionResult<IEnumerable<BankViewModel>> Get([FromQuery]int offset, [FromQuery] int count)
        {
            var Banks = _bankRepo.GetAll(offset,count);
            List<BankViewModel> banksViewModel = new List<BankViewModel>();
            foreach (var bank in Banks)
            {
                banksViewModel.Add(new BankViewModel {
                    Id = bank.Id,
                    BankName = bank.BankShortName,
                    BankIdentificationCode = bank.BankIdentificationCode,
                    
                }); 
            }
            return banksViewModel;
            //return new string[] { "value1", "value2" };
           
        }

        // GET api/BankTwo/BankRegister/Get/5
        [HttpGet("get/{id}")]
        [EnableQuery()]
        public ActionResult<BankViewModel> Get(int id)
        {
            var bank = _bankRepo.Get(id);
            if (bank != null)
            {
                BankViewModel bankViewModel = new BankViewModel
                {
                    Id = bank.Id,
                    BankName = bank.BankShortName,
                    BankIdentificationCode = bank.BankIdentificationCode
                };
                return bankViewModel;
            }
            return NotFound();
        }

        // POST api/BankTwo/BankRegister
        [HttpPost]
        public ActionResult Post([FromBody] BankViewModel bankViewModel)
        {
            BankValidator bankValidator = new BankValidator();
            ValidationResult result = bankValidator.Validate(bankViewModel);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }

            Banks bank = new Banks
            {
                CreatedAt = UnixDateTime.ConvertToDateTime(bankViewModel.CreatedAt),
                LastUpdatedAt = UnixDateTime.ConvertToDateTime(bankViewModel.CreatedAt),
                BankShortName = bankViewModel.BankName,
                BankIdentificationCode = bankViewModel.BankIdentificationCode
            };

            bool dbOperationResult = _bankRepo.Post(bank);
            if (!dbOperationResult)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return CreatedAtAction("Get", bankViewModel);
        }

        // PUT api/BankTwo/BankRegister/Put/5
        [HttpPut("update/{id}")]
        public ActionResult Put(int id, [FromBody] BankViewModel bankViewModel)
        {
            BankValidator bankValidator = new BankValidator();
            ValidationResult result = bankValidator.Validate(bankViewModel);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }

            var bank = _bankRepo.Get(id);
            if (bank != null)
            {
                bank.LastUpdatedAt = UnixDateTime.ConvertToDateTime(bankViewModel.LastUpdated);
                bank.BankShortName = bankViewModel.BankName;

                bool dbOperationResult = _bankRepo.Put(bank);
                if (!dbOperationResult)
                {
                    _logger.LogError(LogEvents.DatabaseError, "Model Not Saved: {id}", id);
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
                _logger.LogInformation(LogEvents.CreatedSuccessfully, "Model {id} Created Successfully", id);
                return Ok();
            }
            _logger.LogWarning(LogEvents.NotFound,"Model {id} doesnt exist",id);
            return NotFound();

        }

        // DELETE api/BankTwo/BankRegister/Delete/5
        [HttpDelete("delete/{id}")]
        public ActionResult Delete(int id)
        {
            var dbOperationResult = _bankRepo.Delete(id);
            if (dbOperationResult)
            {
                return Ok();
            }
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}