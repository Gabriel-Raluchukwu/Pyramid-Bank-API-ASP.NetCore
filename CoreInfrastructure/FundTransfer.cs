using BankTwoAPI_Data.Interfaces;
using BankTwoAPI_Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreInfrastructure
{
    public class FundTransfer
    {
        public ICustomerAccountRepo<CustomerAccount> _customerAccountRepo { get; }

        private const decimal MinimumCurrentAccountBalance = 5000;
        //private const decimal InterTransferBankCharges = 65;

        //Customer Account is a Liability Account to the bank
        // Debit Operation Type = -1 Credit Operation Type = 1
        private const int DebitLiabilityAccountOperation = -1;
        private const int CreditLiabilityAccountOperation = 1;

        //Constructor
        public FundTransfer(ICustomerAccountRepo<CustomerAccount> customerAccountRepo)
        {
            _customerAccountRepo = customerAccountRepo;
        }

        public bool DebitAccount(string accountNumber, decimal Amount,out object error)
        {
            var account = _customerAccountRepo.GetAccountViaAccountNumber(accountNumber);
            if (account == null)
            {
                error = new {error = "Account not Found" };
                return false;
            }

            if (!HasSufficientBalance(account,Amount))
            {
                error = new { error = "Insufficient balance" };
                return false;
            }
            if (!account.IsActive)
            {
                error = new { error = "Account deleted" };
                return false;
            }
            account.AccountBalance += Amount * DebitLiabilityAccountOperation;
            bool dbOperationResult = _customerAccountRepo.Put(account);
            error = new { };
            return dbOperationResult;
        }
        public bool CreditAccount(string accountNumber, decimal Amount)
        {
            var account = _customerAccountRepo.GetAccountViaAccountNumber(accountNumber);
            if (account == null)
            {
                return false;
            }
            if (!account.IsActive)
            {
                return false;
            }
            account.AccountBalance += Amount * CreditLiabilityAccountOperation;
            bool dbOperationResult = _customerAccountRepo.Put(account);
            return dbOperationResult; 
        }
       
        public bool HasSufficientBalance(CustomerAccount account, decimal amount)
        {
            if (account.AccountType == CustomerAccountType.Current )
            {
                if (account.AccountBalance < amount + MinimumCurrentAccountBalance)
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
            if (account.AccountType == CustomerAccountType.Savings)
            {
                if (account.AccountBalance >= amount)
                {
                    return true;
                }
                return false;
            }
            return false;
        }
    }
}
