using BankTwoAPI_Entities.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankTwoAPI_Entities.ViewModels
{
    public class CardRequestViewModel:BaseViewModel
    {
        
        public int CustomerAccountId { get; set; }

        public string CustomerAccountNumber { get; set; }

        //public string CustomerAccountName { get; set; }

        public string Description { get; set; }

        public int CardType { get; set; }

        public int TransactionPin { get; set; }
    }
    
    public class CardRequestValidator : AbstractValidator<CardRequestViewModel>
    {
        public CardRequestValidator()
        {
            RuleFor(cardRequest => cardRequest.CreatedAt).NotNull().NotEmpty();
            RuleFor(cardRequest => cardRequest.CustomerAccountNumber).NotNull().NotEmpty().Length(10)
                .Matches("^[0-9]+$");
         //   RuleFor(cardRequest => cardRequest.CustomerAccountName).NotNull().NotEmpty().Matches("^[a-zA-z ]+$");
            RuleFor(cardRequest => cardRequest.Description).NotNull().NotEmpty().Matches("^[a-zA-z ,.-]+$");
            RuleFor(cardRequest => cardRequest.TransactionPin).NotNull().NotEmpty().Must(Validation.ValidLength);       
        }     
    }
}
