using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BankTwoAPI_Entities.Models
{
    public class BillsPayment
    {
        [ForeignKey("CustomerAccount")]
        public int CustomerAccountId { get; set; }

        public CustomerAccount CustomerAccount { get; set; }

        public string AccountNumber { get; set; }
    }
}
