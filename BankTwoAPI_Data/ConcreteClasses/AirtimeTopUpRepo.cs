using BankTwoAPI_Data.Data;
using BankTwoAPI_Data.Interfaces;
using BankTwoAPI_Entities.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankTwoAPI_Data.ConcreteClasses
{
    public class AirtimeTopUpRepo : IAirtimeTopUpRepo<AirTimeTopUp>
    {
        public BankTwoDatabase _bankTwoDatabase { get; }
        private ILogger _logger { get; }

        public AirtimeTopUpRepo(BankTwoDatabase bankTwoDatabase,ILogger<AirtimeTopUpRepo> logger)
        {
            _bankTwoDatabase = bankTwoDatabase;
            _logger = logger;
        }
        public bool Delete(int id)
        {
            //AirTimeTopUp records cannot be deleted
            //No delete Function Implemented
            
            throw new NotImplementedException();
        }

        public AirTimeTopUp Get(int id)
        {
            var airTimeTopUp = _bankTwoDatabase.AirTimeTopUp.SingleOrDefault(p => p.Id == id);
            return airTimeTopUp;
        }

        public List<AirTimeTopUp> GetAll(int offset,int count)
        {
            var airTimeTopUps = _bankTwoDatabase.AirTimeTopUp.Skip(offset).Take(count).ToList();
            return airTimeTopUps;
        }

        public List<AirTimeTopUp> GetCustomerAirtimeTopUp(string accountNumber, int offset, int count)
        {
            var airTimeTopUps = _bankTwoDatabase.AirTimeTopUp.Where(p => p.CustomerAccountNumber == accountNumber).Skip(offset).Take(count).ToList();
            return airTimeTopUps;
        }

        public bool Post(AirTimeTopUp airTimeTopUp)
        {
            _bankTwoDatabase.AirTimeTopUp.Add(airTimeTopUp);
            try
            {
                _bankTwoDatabase.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool Put(AirTimeTopUp type)
        {
            //AirTimeTopUp records cannot be updated
            //AirTimeTopUp method not Implemented
            throw new NotImplementedException();
        }
    }
}
