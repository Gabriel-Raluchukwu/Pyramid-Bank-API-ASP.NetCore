using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BankTwoAPI_Entities.ViewModels
{
    public class CustomerTransactionPinViewModel
    {
        public int OldTransactionPin { get; set; }

        public int NewTransactionPin { get; set; }

        public int ConfirmTransactionPin { get; set; }
    }

    public class CustomerTransactionPinValidator: AbstractValidator<CustomerTransactionPinViewModel>
    {
        public CustomerTransactionPinValidator()
        {
            RuleFor(pin => pin.OldTransactionPin).NotNull().NotEmpty().Must(Validation.ValidLength);
            RuleFor(pin => pin.NewTransactionPin).NotNull().NotEmpty().Must(Validation.ValidLength);
            RuleFor(pin => pin.ConfirmTransactionPin).Equal(pin => pin.NewTransactionPin);
        }
    }
}
