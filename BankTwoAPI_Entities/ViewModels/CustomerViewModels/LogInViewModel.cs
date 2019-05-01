using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankTwoAPI_Entities.ViewModels
{
    public class LogInViewModel
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public bool IsAuthenticated { get; set; }
    }

    public class LogInValidator : AbstractValidator<LogInViewModel>
    {
        public LogInValidator()
        {
            RuleFor(customer => customer.UserName).NotNull().NotEmpty()
               .Length(4, 20).Matches("^[a-zA-z 0-9_-]+$");
            RuleFor(customer => customer.Password).NotNull().NotEmpty()
                .MinimumLength(8);
        }
    }
}
