using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BankTwoAPI_Entities.Models
{
    public class AccountCustomerCategory
    {
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }

        public Customer Customer { get; set; }

        [ForeignKey("CUstomerAccount")]
        public int CustomerAccountId { get; set; }

        public CustomerAccount CustomerAccount { get; set; }

    }
}
