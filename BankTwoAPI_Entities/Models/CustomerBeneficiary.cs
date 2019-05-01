using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BankTwoAPI_Entities.Models
{
    public class CustomerBeneficiary:BaseEntity
    {
        public DateTime LastUpdatedAt { get; set; }

        [ForeignKey("CustomerAccount")]
        public int CustomerId { get; set; }

        public CustomerAccount CustomerAccount { get; set; }

        public string CustomerAccountNumber { get; set; }

        [ForeignKey("Banks")]
        public int BankId { get; set; }

        public Banks Banks { get; set; }

        public string RecipientAccountNumber { get; set; }

        public string RecipientAccountName { get; set; }

        public string NickName { get; set; }

    }
}
