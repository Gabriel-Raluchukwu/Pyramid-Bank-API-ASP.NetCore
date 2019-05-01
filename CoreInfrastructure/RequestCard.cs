using BankTwoAPI_Data.Interfaces;
using BankTwoAPI_Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreInfrastructure
{
    public class RequestCard
    {
        public FundTransfer _fundTransfer { get; }

        private readonly decimal VerveCardCharges = 500.0m;
        private readonly decimal MasterCardCharges = 500.0m;
        private readonly decimal DollarMasterCardCharges = 1500.0m;

        //Constructor
        public RequestCard(ICustomerAccountRepo<CustomerAccount> customerAccountRepo)
        {
            _fundTransfer = new FundTransfer(customerAccountRepo);
        }

        public bool CardChargeDebit( string accountNumber,CardType cardType)
        {
            bool SuccessfulCardDebit  = false;
            //var customerAccount = _customerAccountRepo.GetAccountViaAccountNumber(accountNumber);
           
            switch (cardType)
            {
                case CardType.VerveCard:
                   SuccessfulCardDebit =  _fundTransfer.DebitAccount(accountNumber,VerveCardCharges, out object error);
                    break;
                case CardType.MasterCard:
                    SuccessfulCardDebit = _fundTransfer.DebitAccount(accountNumber,MasterCardCharges, out object error2);
                    break;
                case CardType.DollarMasterCard:
                    SuccessfulCardDebit = _fundTransfer.DebitAccount(accountNumber, DollarMasterCardCharges, out object error3);
                    break;
                default:
                    break;
            }
            if (SuccessfulCardDebit)
            {
                return true;
            }
            return false;
        }

    }
}
