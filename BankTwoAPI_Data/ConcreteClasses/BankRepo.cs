using BankTwoAPI_Data.Data;
using BankTwoAPI_Data.Interfaces;
using System;
using System.Collections.Generic;
using BankTwoAPI_Entities.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace BankTwoAPI_Data.ConcreteClasses
{
    public class BankRepo : IBankRepo<Banks>
    {
        public BankTwoDatabase _bankTwoDatabase { get; }
        private ILogger _logger { get; }

        public BankRepo(BankTwoDatabase bankTwoDatabase,ILogger<BankRepo> logger)
        {
            _bankTwoDatabase = bankTwoDatabase;
            _logger = logger;
        }    

        public bool Delete(int id)
        {
            var bank = _bankTwoDatabase.Banks.Find(id);
    
            try
            {
                bank.IsActive = false;
                _logger.LogInformation("Delete: Saving Bank to Database", new { bank.BankShortName, bank.BankIdentificationCode });
                _bankTwoDatabase.SaveChanges();
            }
            catch (DbUpdateConcurrencyException e)
            {
                _logger.LogError(LogEvents.DatabaseError, e, e.Message,new { bank.BankShortName ,bank.BankIdentificationCode});
                return false;
            }
            catch (Exception e)
            {
                _logger.LogError(LogEvents.DatabaseError, e, e.Message, new { bank.BankShortName, bank.BankIdentificationCode });
                return false;
            }
            return true;
        }

        public Banks Get(int id)
        {
            Banks Bank = _bankTwoDatabase.Banks.Where(p => p.IsActive == true).SingleOrDefault(p => p.Id == id);
            return Bank;
        }

        public List<Banks> GetAll(int offset,int count)
        {
            var banks = _bankTwoDatabase.Banks.Where(p => p.IsActive == true).Skip(offset).Take(count).ToList();
            return banks;
        }

        public bool Post(Banks bank)
        {
            _bankTwoDatabase.Banks.Add(bank);
            try
            {
                _logger.LogInformation("Saving Bank to Database",bank);
                _bankTwoDatabase.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.LogError(LogEvents.DatabaseError, e, e.Message, bank);
                return false;
            }
            return true;
        }

        public bool Put(Banks bank)
        {
            _bankTwoDatabase.Attach(bank).State = EntityState.Modified;

            try
            {
                _logger.LogInformation("Saving Bank update to Database", bank);
                _bankTwoDatabase.SaveChanges();
            }
            catch (DbUpdateConcurrencyException e)
            {
                _logger.LogError(LogEvents.DatabaseUpdateConcurrencyError,e,e.Message,bank);
                return false;
            }
            catch (Exception e)
            {
                _logger.LogError(LogEvents.DatabaseError,e,e.Message,bank);
                return false;
            }

            return true;
        }
    }
}
