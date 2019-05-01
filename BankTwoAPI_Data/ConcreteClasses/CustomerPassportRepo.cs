using BankTwoAPI_Data.Data;
using BankTwoAPI_Data.Interfaces;
using BankTwoAPI_Entities.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankTwoAPI_Data.ConcreteClasses
{
    public class CustomerPassportRepo : ICustomerPassportRepo<CustomerPassport>
    {
        public BankTwoDatabase _bankTwoDatabase { get; }

        private ILogger _logger { get; }

        //Constructor
        public CustomerPassportRepo(BankTwoDatabase bankTwoDatabase, ILogger<CustomerPassportRepo> logger)
        {
            _bankTwoDatabase = bankTwoDatabase;
            _logger = logger;
        }

        public bool Post(CustomerPassport type)
        {
            throw new NotImplementedException();
        }

        public CustomerPassport Get(int id)
        {
            throw new NotImplementedException();
        }

        public List<CustomerPassport> GetAll(int offset,int count)
        {
            throw new NotImplementedException();
        }

        public bool Put(CustomerPassport type)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
