using BankTwoAPI_Entities.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BankTwoAPI_Entities.ViewModels
{
    public class MobileTopUpViewModel:BaseViewModel
    {
        public MobileNetworks MobileNetworks { get; set; }

        [ForeignKey("MobileNetwork")]
        public int MobileNetworkId { get; set; }

        public decimal Amount { get; set; }

        public string MobileNumber { get; set; }

        public string CustomerAccountNumber { get; set; }

        public int TransactionPin { get; set; }

    }

    public class MobileTopUpValidator : AbstractValidator<MobileTopUpViewModel>
    {
        public MobileTopUpValidator()
        {
            RuleFor(mobileTopUp => mobileTopUp.CreatedAt).NotNull().NotEmpty();
            RuleFor(mobileTopUp => mobileTopUp.CustomerAccountNumber).NotNull().NotEmpty().Length(10).Matches("^[0-9]+");
            RuleFor(mobileTopUp => mobileTopUp.Amount).NotNull().NotEmpty().GreaterThan(0.0M);
            RuleFor(mobileTopUp => mobileTopUp.MobileNetworkId).NotNull().NotEmpty();
            RuleFor(mobileTopUp => mobileTopUp.TransactionPin).NotNull().NotEmpty().Must(Validation.ValidLength);
        }
    }
}
