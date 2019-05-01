using BankTwoAPI_Data.Data;
using BankTwoAPI_Data.Interfaces;
using BankTwoAPI_Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankTwoAPI_Data.ConcreteClasses
{
    public class CustomerBeneficiaryRepo:ICustomerBeneficiaryRepo<CustomerBeneficiary>
    {
        public BankTwoDatabase _bankTwoDatabase { get; set; }
        private ILogger _logger { get; }

        public CustomerBeneficiaryRepo(BankTwoDatabase bankTwoDatabase,ILogger<CustomerBeneficiaryRepo> logger)
        {
            _bankTwoDatabase = bankTwoDatabase;
            _logger = logger;
        }

        public bool Delete(int id)
        {
            var beneficiary = _bankTwoDatabase.CustomerBeneficiaries.Find(id);

            try
            {
                beneficiary.IsActive = false;
                _logger.LogInformation("Delete: Saving Customer beneficiary update to database");
                _bankTwoDatabase.SaveChanges();
            }
            catch (DbUpdateConcurrencyException e)
            {
                _logger.LogError(LogEvents.DatabaseUpdateConcurrencyError,e,e.Message,new { beneficiary.CustomerAccountNumber });
                return false;
            }
            catch (Exception e)
            {
                _logger.LogError(LogEvents.DatabaseError,e,e.Message,new { beneficiary.CustomerAccountNumber});
                return false;
            }
            return true;
        }

        public CustomerBeneficiary Get(int id)
        {
            var beneficiary = _bankTwoDatabase.CustomerBeneficiaries.Where(p => p.IsActive == true).SingleOrDefault(p => p.Id == id);
            return beneficiary;
        }

        public List<CustomerBeneficiary> GetCustomerBeneficiaries(string accountNumber, int offset, int count)
        {
            var customerBeneficiaries = _bankTwoDatabase.CustomerBeneficiaries.Where(p => p.CustomerAccountNumber == accountNumber)
                .Where(p => p.IsActive == true).Skip(offset).Take(count).ToList();
            return customerBeneficiaries;
        }

        public List<CustomerBeneficiary> GetAll(int offset, int count)
        {
            var Beneficiaries = _bankTwoDatabase.CustomerBeneficiaries.Where( p => p.IsActive == true)
                .Skip(offset).Take(count).Include(p => p.Banks).ToList();
            return Beneficiaries;
        }

        public bool Post(CustomerBeneficiary customerBeneficiary)
        {
            _bankTwoDatabase.CustomerBeneficiaries.Add(customerBeneficiary);
            try
            {
                _logger.LogInformation("Saving beneficiary to database",customerBeneficiary);
                _bankTwoDatabase.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.LogError(LogEvents.DatabaseError,e,e.Message,customerBeneficiary);
                return false;
            }
            return true;
        }

        public bool Put(CustomerBeneficiary type)
        {
            //No Update Implemented for Beneficiaries
            //Beneficary cannot be updated
            throw new NotImplementedException();
        }
    }
}
