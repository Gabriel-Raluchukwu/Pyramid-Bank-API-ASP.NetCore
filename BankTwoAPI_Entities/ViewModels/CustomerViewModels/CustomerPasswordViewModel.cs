using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BankTwoAPI_Entities.ViewModels
{
    public class CustomerPasswordViewModel
    {
        public long LastUpdated { get; set; }

        public string OldPassword { get; set; }

        public string NewPassword { get; set; }

        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }

    }

    public class CustomerPasswordValidator : AbstractValidator<CustomerPasswordViewModel>
    {
        public CustomerPasswordValidator()
        {
            RuleFor(password => password.LastUpdated).NotNull().NotEmpty();
            RuleFor(password => password.OldPassword).NotNull().NotEmpty();
            RuleFor(password => password.NewPassword).NotNull().NotEmpty().MinimumLength(8);
            RuleFor(password =>password.ConfirmPassword).Equal(password => password.NewPassword);
        }          
    }
}
