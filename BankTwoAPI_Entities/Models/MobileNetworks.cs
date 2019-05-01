using System;
using System.Collections.Generic;
using System.Text;

namespace BankTwoAPI_Entities.Models
{
    public class MobileNetworks:BaseEntity
    {
        public DateTime LastUpdatedAt { get; set; }

        public string MobileNetworkName { get; set; }

        public int NetworkProviderRegistratonCode { get; set; }
    }
}
