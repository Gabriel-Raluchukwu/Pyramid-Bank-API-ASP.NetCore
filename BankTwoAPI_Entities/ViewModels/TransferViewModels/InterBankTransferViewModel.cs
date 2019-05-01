using BankTwoAPI_Entities.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BankTwoAPI_Entities.ViewModels
{
    public class InterBankTransferViewModel:BaseViewModel
    {
        public int BanksId { get; set; }

        public string BankName { get; set; }

        public string CustomerAccountNumber { get; set; }

        public decimal TransferAmount { get; set; }

        public string Description { get; set; }

        public string RecipientAccountName { get; set; }

        public string RecipientAccountNumber { get; set; }

        public int TransactionPin { get; set; }

    }

    public class InterBankTransferValidator : AbstractValidator<InterBankTransferViewModel>
    {
        public InterBankTransferValidator()
        {
            RuleFor(interBankTransfer => interBankTransfer.CreatedAt).NotNull().NotEmpty();
            RuleFor(interBankTransfer => interBankTransfer.BankName).NotNull().NotEmpty()
                .Matches("^[a-zA-Z ]+$"); ;
            RuleFor(interBankTransfer => interBankTransfer.CustomerAccountNumber).NotNull().NotEmpty()
                .Length(10).Matches("^[0-9]+$"); ;
            RuleFor(interBankTransfer => interBankTransfer.TransferAmount).NotNull().NotEmpty().GreaterThan(0.0M);
            RuleFor(interBankTransfer => interBankTransfer.Description).NotNull().NotEmpty()
                .Matches("^[a-zA-Z ,.-]+$"); ;
            RuleFor(interBankTransfer => interBankTransfer.RecipientAccountName).NotNull().NotEmpty()
                .Matches("^[a-zA-Z ]+$"); ;
            RuleFor(interBankTransfer => interBankTransfer.RecipientAccountNumber).NotNull().NotEmpty()
                .Length(10).Matches("^[0-9]+$");
            RuleFor(interBankTransfer => interBankTransfer.TransactionPin).NotNull().NotEmpty().Must(Validation.ValidLength);
        }
    }
}
