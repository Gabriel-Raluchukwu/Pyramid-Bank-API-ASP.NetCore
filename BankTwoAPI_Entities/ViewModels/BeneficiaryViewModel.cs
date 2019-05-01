using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankTwoAPI_Entities.ViewModels
{
    public class BeneficiaryViewModel:BaseViewModel
    {
        public long LastUpdatedAt { get; set; }

        public int CustomerId { get; set; }

        public string CustomerAccountNumber { get; set; }

        public int BankId { get; set; }

        public string BankShortName { get; set; }

        public string RecipientAccountNumber { get; set; }

        public string RecipientAccountName { get; set; }

        public string NickName { get; set; }
    }

    public class BeneficiaryValidator : AbstractValidator<BeneficiaryViewModel>
    {
        public BeneficiaryValidator()
        {
            RuleFor(cardRequest => cardRequest.CreatedAt).NotNull().NotEmpty();
            RuleFor(beneficiary => beneficiary.CustomerId).NotEmpty();
            RuleFor(beneficiary => beneficiary.CustomerAccountNumber).NotNull().NotEmpty()
                .Length(10).Matches("^[0-9]+$");
            RuleFor(beneficiary => beneficiary.BankId).NotEmpty();
            RuleFor(beneficiary => beneficiary.RecipientAccountName).NotNull().NotEmpty()
                .Matches("^[a-zA-z ]+$");
            RuleFor(beneficiary => beneficiary.RecipientAccountNumber).NotNull().NotEmpty()
                .Length(10).Matches("^[0-9]+$");
            RuleFor(beneficiary => beneficiary.NickName).NotNull().NotEmpty()
                .Matches("^[a-zA-z ]+$"); ;
        }
    }
}
