using BankTwoAPI_Entities.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BankTwoAPI_Entities.ViewModels
{
    public class CustomerViewModel:BaseViewModel
    {
        public long LastUpdated { get; set; }

        public string UserName { get; set; }

        public string CustomerAccountNumber { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public int VerificationCode { get; set; }

    }

    public class CustomerValidator : AbstractValidator<CustomerViewModel>
    {
        public CustomerValidator()
        {
            RuleFor(customer => customer.CreatedAt).NotNull().NotEmpty();
            RuleFor(customer => customer.UserName).NotNull().NotEmpty()
                .Length(4,20).Matches("^[a-zA-z 0-9_-]+$");
            RuleFor(customer => customer.Password).NotNull().NotEmpty()
                .MinimumLength(8);
            RuleFor(customer => customer.ConfirmPassword).Equal(customer => customer.Password);
            RuleFor(customer => customer.CustomerAccountNumber).NotNull().NotEmpty()
                .Length(10).Matches("^[0-9]+$");
            RuleFor(customer => customer.VerificationCode).Must(ValidLength);
        }

        private bool ValidLength(int VerificationCode)
        {
            const int verificationCodeLength = 6;
            string verificationCode = VerificationCode.ToString();
            return (verificationCode.Length == verificationCodeLength) ? true : false;
        }
    }
}
