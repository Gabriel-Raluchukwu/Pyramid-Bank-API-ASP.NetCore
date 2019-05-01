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
    public class CustomerRepo : ICustomerRepo<Customer>
    {
        public BankTwoDatabase _bankTwoDatabase { get;}
        private ILogger _logger { get; }

        public CustomerRepo(BankTwoDatabase bankTwoDatabase,ILogger<CustomerRepo> logger)
        {
            _bankTwoDatabase = bankTwoDatabase;
            _logger = logger;
        }  
        public bool Delete(int id)
        {
            var customer = _bankTwoDatabase.Customers.Find(id);
        
            try
            {
                customer.IsActive = false;
                _logger.LogInformation("Delete: Saving customer update to Database",new { customer.CustomerAccountNumber });
                _bankTwoDatabase.SaveChanges();
            }
            catch (DbUpdateConcurrencyException e)
            {
                _logger.LogError(LogEvents.DatabaseError, e, e.Message, new {customer.CustomerAccountNumber });
                return false;
            }
            catch (Exception e)
            {
                _logger.LogError(LogEvents.DatabaseError, e, e.Message, new { customer.CustomerAccountNumber});
                return false;
            }
            return true;
        }

        public Customer Get(int id)
        {
            var customer = _bankTwoDatabase.Customers.Where(p => p.IsActive == true).SingleOrDefault(p => p.Id == id);
            return customer;
        }

        public List<Customer> GetAll(int offset, int count)
        {
            var customers = _bankTwoDatabase.Customers.Where(p => p.IsActive == true).Skip(offset).Take(count).ToList();
            return customers;
        }

        public bool Post(Customer customer)
        {
            _bankTwoDatabase.Customers.Add(customer);

            try
            {
                _logger.LogInformation("Saving customer to database", customer);
                AddCustomerToJoinTable(customer);
               // _bankTwoDatabase.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.LogError(LogEvents.DatabaseError, e, e.Message, customer);
                return false;
            }
            return true;
        }

        public bool Put(Customer customer)
        {
            _bankTwoDatabase.Entry(customer).State = EntityState.Modified;
            try
            {
                _logger.LogInformation("Saving customer update to database",customer);
                _bankTwoDatabase.SaveChanges();
            }
            catch (DbUpdateConcurrencyException e)
            {
                _logger.LogError(LogEvents.DatabaseError,e,e.Message,customer);
                return false;
            }
            catch (Exception e)
            {
                _logger.LogError(LogEvents.DatabaseError,e,e.Message,customer);
                return false;
            }
            return true;
        }
        public Customer GetByUsername(string username)
        {
            var customer = _bankTwoDatabase.Customers.Where(p => p.IsActive == true).SingleOrDefault(p => p.UserName == username);
            return customer;
        }
        private void AddCustomerToJoinTable(Customer customer)
        {
            var customerAccount = _bankTwoDatabase.CustomerAccounts.Where(p => p.IsActive == true)
                .SingleOrDefault(p => p.CustomerAccountNumber == customer.CustomerAccountNumber );

            if (customerAccount != null)
            {
                _bankTwoDatabase.AccountCustomerCategories.Add(new AccountCustomerCategory
                {
                    CustomerId = customer.Id,
                    CustomerAccountId = customerAccount.Id
                });
            }
            else
            {
                _logger.LogError("Customer Account not found",new { customer.CustomerAccountNumber });
                throw new Exception("Customer and CustomerAccount join-table operation Failed");
            }

            try
            {
                _logger.LogInformation("Saving Customer and Account to Join table",new {customer.UserName, customer.CustomerAccountNumber });
                _bankTwoDatabase.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.LogError(LogEvents.DatabaseError,e,e.Message,customer);
                throw new Exception("Customer and CustomerAccount join-table operation Failed");
            }
        }
    }
}
