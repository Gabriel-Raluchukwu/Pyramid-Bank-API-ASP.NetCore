using BankTwoAPI_Entities.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BankTwoAPI_Entities.ViewModels
{
    public class CustomerAccountViewModel : BaseViewModel
    {
        public long LastUpdated { get; set; }

        public string CustomerAccountNumber { get; set; }

        public string Surname { get; set; }

        public string FirstName { get; set; }

        public string OtherNames { get; set; }

        public string BVNNumber { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public int? AccountType { get; set; }

        public decimal AccountBalance { get; set; }

        public int TransactionPin { get; set; }

        public int ConfirmTransactionPin { get; set; }

        [ForeignKey("CustomerPassport")]
        public int CustomerPassportId { get; set; }

        public CustomerPassport CustomerPassport { get; set; }

        public virtual ICollection<AccountCustomerCategory> Customers { get; set; }
    }

    public class CustomerAccountValidator:AbstractValidator<CustomerAccountViewModel>
    {
        public CustomerAccountValidator()
        {
            RuleFor(account => account.CreatedAt).NotNull().NotEmpty();
            RuleFor(account => account.Surname).NotNull().NotEmpty()
                .Matches("^[a-zA-z]+$");
            RuleFor(account => account.FirstName).NotNull()
                .NotEmpty().Matches("^[a-zA-Z]+$");
            RuleFor(account => account.OtherNames).NotNull()
                .NotEmpty().Matches("^[a-zA-Z -]+$");
            RuleFor(account => account.PhoneNumber).NotNull().NotEmpty()
                .Length(10).Matches("^[0-9]+$");
            RuleFor(account => account.Email).NotNull().NotEmpty().EmailAddress();
            //RuleFor(account => account.AccountType).NotNull().IsInEnum();
            RuleFor(account => account.TransactionPin).NotNull().Must(Validation.ValidLength);
            RuleFor(account => account.ConfirmTransactionPin).Equal(account => account.TransactionPin);
            RuleFor(account => account.BVNNumber).Length(10)
                .Matches("^[0-9]+$");
            
        }
    }
}
