using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BankTwoAPI_Entities.ViewModels
{

    public class IntraBankTransferViewModel:BaseViewModel
    {
        public int CustomerAccountId { get; set; }

        public string CustomerAccountNumber { get; set; }

        public decimal TransferAmount { get; set; }

        public string Description { get; set; }

        public int RecipientBankId { get; set; }

        public string RecipientBankName { get; set; }

        public string RecipientAccountName { get; set; }

        public string RecipientAccountNumber { get; set; }

        [Required]
        public int TransactionPin { get; set; }
    }
    
    public class IntraBankTransferValidator : AbstractValidator<IntraBankTransferViewModel>
    {
        public IntraBankTransferValidator()
        {
            RuleFor(intraBankTransfer => intraBankTransfer.CreatedAt).NotNull().NotEmpty();
            RuleFor(intraBankTransfer => intraBankTransfer.CustomerAccountNumber).NotNull().NotEmpty()
                .Length(10).Matches("^[0-9]+$"); ;
            RuleFor(intraBankTransfer => intraBankTransfer.TransferAmount).NotNull().NotEmpty().GreaterThan(0.0M);
            RuleFor(intraBankTransfer => intraBankTransfer.Description).NotNull().NotEmpty()
                .Matches("^[a-zA-Z ,.-]+$"); ;
            RuleFor(intraBankTransfer => intraBankTransfer.RecipientAccountName).NotNull().NotEmpty()
                .Matches("^[a-zA-Z ]+$"); ;
            RuleFor(intraBankTransfer => intraBankTransfer.RecipientAccountNumber).NotNull().NotEmpty()
                .Length(10).Matches("^[0-9]+$");
            RuleFor(intraBankTransfer => intraBankTransfer.RecipientBankId).NotNull().NotEmpty().GreaterThan(0);
            //RuleFor(intraBankTransfer => intraBankTransfer.RecipientBankName);
            RuleFor(intraBankTransfer => intraBankTransfer.TransactionPin).NotNull().NotEmpty().Must(Validation.ValidLength);
        }
    }
}
