using BankTwoAPI_Data.Interfaces;
using BankTwoAPI_Entities.Models;
using BankTwoAPI_Entities.ViewModels;
using BankTwoCoreAPI.UtilityLogic;
using FluentValidation.Results;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static BankTwoCoreAPI.UtilityLogic.PasswordHash;

namespace BankTwoCoreAPI.Controllers
{
    [Route("api/aurora/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        public ICustomerRepo<Customer> _customerRepo { get; }

        private ILogger _logger { get; }
        private IConfiguration Configuration { get; }

        public CustomerController(ICustomerRepo<Customer> customerRepo,ILogger<CustomerController> logger,
            IConfiguration configuration)
        {
            _customerRepo = customerRepo;
            _logger = logger;
            Configuration = configuration;
        }

        // GET: api/Customer
        [HttpGet]
        [EnableQuery()]
        public ActionResult<IEnumerable<CustomerViewModel>> Get([FromQuery]int offset, [FromQuery] int count)
        {
            List<CustomerViewModel> customers = new List<CustomerViewModel>();
            var AllCustomers = _customerRepo.GetAll(offset,count);
            foreach (var customer in AllCustomers)
            {
                //TODO: implement This
                customers.Add(
                    new CustomerViewModel
                    {
                        Id = customer.Id,
                        CreatedAt = UnixDateTime.ConvertToUnixTime(customer.CreatedAt),
                        CustomerAccountNumber = customer.CustomerAccountNumber,
                        UserName = customer.UserName                         
                    });
            }
            return customers;
        }

        // GET: api/Customer/5
        [HttpGet("get/{id}")]
        [EnableQuery()]
        public ActionResult<CustomerViewModel> Get(int id)
        {
            var customer = _customerRepo.Get(id);
            if (customer != null)
            {
                CustomerViewModel customerViewModel = new CustomerViewModel
                {
                    Id = customer.Id,
                    UserName = customer.UserName
                };

                return customerViewModel;
            }
            return NotFound();
        }

        // POST: api/Customer
        [HttpPost]
        public ActionResult Post([FromBody] CustomerViewModel customerViewModel)
        {
            CustomerValidator customerValidator = new CustomerValidator();
            ValidationResult result = customerValidator.Validate(customerViewModel);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }
            
            //Generating salt
            string passwordSalt = Guid.NewGuid().ToString().Replace("-", "");

            Customer customer = new Customer
            {
                CreatedAt = UnixDateTime.ConvertToDateTime(customerViewModel.CreatedAt),
                LastUpdatedAt = UnixDateTime.ConvertToDateTime(customerViewModel.CreatedAt),
                CustomerAccountNumber = customerViewModel.CustomerAccountNumber,
                UserName = customerViewModel.UserName,
                PasswordHash = PasswordHash.HashPassword(customerViewModel.Password,passwordSalt),
                PasswordSalt = passwordSalt,
                VerificationCode = customerViewModel.VerificationCode
            };
            bool dbOperationResult = _customerRepo.Post(customer);
            if (!dbOperationResult)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return CreatedAtAction("Get", customerViewModel); ;
        }

        [Route("Login")]
        public ActionResult Login([FromBody] LogInViewModel logInViewModel)
        {
            //FIXME: Implement Login Method
            //check is customers account is active before login us successful
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var customer = Authenticate(logInViewModel.UserName,logInViewModel.Password);
            if (customer == null)
            {
                return BadRequest(new { message = " Username or Password is incorrect"});
            }
            // JwtSecurityTokenHandler
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretkey = Configuration["AuthenticationSecurity:Secret"];
            var key = System.Text.Encoding.ASCII.GetBytes(secretkey);
            
            // Security Token Descriptor
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                IssuedAt = DateTime.UtcNow,
                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,customer.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(4),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)

            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return Ok(new {
                Id = customer.Id,
                Username = customer.UserName,
                AccountNumber = customer.CustomerAccountNumber,
                Token = tokenString
            });
        }

        [HttpPut("update/{id}")]
        public ActionResult Put(int id, [FromBody] CustomerPasswordViewModel passwordViewModel)
        {
            var customer = _customerRepo.Get(id);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (customer != null)
            {
                string oldPasswordHash = PasswordHash.HashPassword(passwordViewModel.OldPassword, customer.PasswordSalt);
                if (customer.PasswordHash == oldPasswordHash)
                {
                    //Generating salt
                    string passwordSalt = Guid.NewGuid().ToString().Replace("-", "");

                    customer.PasswordHash = PasswordHash.HashPassword(passwordViewModel.NewPassword,passwordSalt);
                    customer.PasswordSalt = passwordSalt;
                    bool dbOperationResult = _customerRepo.Put(customer);
                    if (!dbOperationResult)
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError);
                    }
                }
                return Ok();
            }
            return NotFound();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("delete/{id}")]
        public ActionResult Delete(int id)
        {
            bool dbOperationResult = _customerRepo.Delete(id);
            if (!dbOperationResult)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok();
        }

        private Customer Authenticate(string username, string password)
        {
            var customer = _customerRepo.GetByUsername(username);
            if (customer == null)
            {
                //throw new Exception("customer not found");
                return null;
            }
            var check = PasswordHash.VerifyPassword(password,customer.PasswordHash,customer.PasswordSalt);
            if (check == Authentication.failure)
            {
                return null;
            }
            return customer;
        }
    }
}
