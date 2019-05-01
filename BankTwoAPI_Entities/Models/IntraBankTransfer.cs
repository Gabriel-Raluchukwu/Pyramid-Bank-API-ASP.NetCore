using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BankTwoAPI_Entities.Models
{
    public class IntraBankTransfer:BaseEntity
    {
        [ForeignKey("CustomerAccount")]
        public int CustomerAccountId { get; set; }

        public string CustomerAccountNumber { get; set; }

        public CustomerAccount CustomerAccount { get; set; }

        public Banks Banks { get; set; }

        public decimal TransferAmount { get; set; }

        public string Description { get; set; }

        [ForeignKey("Banks")]
        public int RecipientBankId { get; set; }

        public string RecipientBankName { get; set; }

        public string RecipientAccountName { get; set; }

        public string RecipientAccountNumber { get; set; }
    }
}
