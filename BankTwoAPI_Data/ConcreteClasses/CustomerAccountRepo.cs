using BankTwoAPI_Data.Data;
using BankTwoAPI_Data.Interfaces;
using BankTwoAPI_Entities.Models;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace BankTwoAPI_Data.ConcreteClasses
{
    public class CustomerAccountRepo : ICustomerAccountRepo<CustomerAccount>
    {
        public BankTwoDatabase _bankTwoDatabase { get; }
        private ILogger _logger { get; }

        public CustomerAccountRepo(BankTwoDatabase bankTwoDatabase,ILogger<CustomerAccountRepo> logger)
        {
            _bankTwoDatabase = bankTwoDatabase;
            _logger = logger;
        }
        public bool Delete(int id)
        {
            var customerAccount = _bankTwoDatabase.CustomerAccounts.Find(id);
            try
            {
                customerAccount.IsActive = false;
                _logger.LogInformation("Delete: Saving Customer account update to Database", new { customerAccount.CustomerAccountNumber });
                _bankTwoDatabase.SaveChanges();
            }
            catch (DbUpdateConcurrencyException e)
            {
                _logger.LogError(LogEvents.DatabaseUpdateConcurrencyError, e, e.Message, new { customerAccount.CustomerAccountNumber });
                return false;
            }
            catch (Exception e)
            {
                _logger.LogError(LogEvents.DatabaseUpdateConcurrencyError, e, e.Message, new { customerAccount.CustomerAccountNumber });
                return false;
            }
            return true;
        }

        public CustomerAccount Get(int id)
        {

            var customerAccount = _bankTwoDatabase.CustomerAccounts.Where(p => p.IsActive == true).SingleOrDefault(p => p.Id == id);
            return customerAccount;
        }

        //Implement GetViaAccountNumber
        public CustomerAccount GetAccountViaAccountNumber(string accountNumber)
        {
            var customerAccount = _bankTwoDatabase.CustomerAccounts.Where(p => p.IsActive == true).SingleOrDefault(p => p.CustomerAccountNumber == accountNumber);
            return customerAccount;
        }

        public List<CustomerAccount> GetAll(int offset, int count)
        {
            var customerAccounts = _bankTwoDatabase.CustomerAccounts.Skip(offset).Take(count).ToList();
            return customerAccounts;
        }

        public bool Post(CustomerAccount customerAccount)
        {
            _bankTwoDatabase.CustomerAccounts.Add(customerAccount);
            try
            {
                _logger.LogInformation("Saving Customer account to Database", customerAccount);
                _bankTwoDatabase.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.LogError(LogEvents.DatabaseError,e,e.Message,customerAccount);
                return false;
            }
            _logger.LogError(LogEvents.SavedToDatabase,"Saved Successfully to the Database",customerAccount);
            return true;
        }

        public bool Put(CustomerAccount customerAccount)
        {
            _bankTwoDatabase.Entry(customerAccount).State = EntityState.Modified;
            try
            {
                _logger.LogInformation("Saving Customer account update to Database", customerAccount);
                _bankTwoDatabase.SaveChanges();
            }
            catch (DbUpdateConcurrencyException e)
            {
                _logger.LogError(LogEvents.DatabaseUpdateConcurrencyError, e, e.Message, customerAccount);
                return false;
            }
            catch (Exception e)
            {
                _logger.LogError(LogEvents.DatabaseError, e, e.Message, customerAccount);
                return false;
            }
          //  _logger.LogError(LogEvents.SavedToDatabase, "Update Successfully saved to the Database", customerAccount);
            return true;
        }

       
    }
}
