using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankTwoAPI_Entities.ViewModels
{
    public class BankViewModel:BaseViewModel
    {
        public long LastUpdated { get; set; }

        public string BankName { get; set; }

        public int BankIdentificationCode { get; set; }
    }

    public class BankValidator : AbstractValidator<BankViewModel>
    {
        public BankValidator()
        {
            RuleFor(bank => bank.BankName).NotNull().NotEmpty()
                .Matches("^[a-zA-Z ]+$");
            RuleFor(bank => bank.BankIdentificationCode).NotNull().Must(ValidLength);
        }

        private bool ValidLength(int IdentificationCode)
        {
            const int verificationCodeLength = 6;
            string verificationCode = IdentificationCode.ToString();
            return (verificationCode.Length == verificationCodeLength) ? true : false;
        }
    }
}
