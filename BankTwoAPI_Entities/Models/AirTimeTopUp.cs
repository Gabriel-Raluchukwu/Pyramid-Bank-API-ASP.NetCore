using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BankTwoAPI_Entities.Models
{
    public class AirTimeTopUp:BaseEntity
    {
        [ForeignKey("CustomerAccount")]
        public int CustomerAccountId { get; set; }

        public CustomerAccount CustomerAccount { get; set; }

        public string CustomerAccountNumber { get; set; }

        [ForeignKey("MobileNetworks")]
        public int MobileNetworksId { get; set; }

        public MobileNetworks MobileNetworks { get; set; }

        public string MobileNumber { get; set; }

        public decimal Amount { get; set; }

    }
}
