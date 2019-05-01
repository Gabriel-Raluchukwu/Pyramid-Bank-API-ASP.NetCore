using System;
using System.Collections.Generic;
using System.Text;

namespace BankTwoAPI_Entities.Models
{
    public class Banks:BaseEntity
    {
        public DateTime LastUpdatedAt { get; set; }

        public string BankFullName { get; set; }

        public string BankShortName { get; set; }

        public int BankIdentificationCode { get; set; }
    }
}
