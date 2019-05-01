using BankTwoAPI_Data.Interfaces;
using BankTwoAPI_Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreInfrastructure
{
    public class PinAuthentication
    {
        public ICustomerAccountRepo<CustomerAccount> _customerAccountRepo { get; }

        //Constructor
        public PinAuthentication(ICustomerAccountRepo<CustomerAccount> customerAccountRepo)
        {
            _customerAccountRepo = customerAccountRepo;
        }

        public bool AuthenticateAccountPin(string accountNumber, int accountTransactionPin)
        {
            var account = _customerAccountRepo.GetAccountViaAccountNumber(accountNumber);
            if (account == null)
            {
                return false;
            }
            if (account.TransactionPin == accountTransactionPin)
            {
                return true;
            }
            return false;
        }
    }
}
