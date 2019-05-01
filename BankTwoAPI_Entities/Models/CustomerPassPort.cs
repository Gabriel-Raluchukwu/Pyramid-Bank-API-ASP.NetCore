using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BankTwoAPI_Entities.Models
{
    public class CustomerPassport : BaseEntity
    {
        public CustomerAccount CustomerAccount { get; set; }

        [ForeignKey("CustomerAccount")]
        public int CustomerAccountId { get; set; }

        public DateTime LastUpdatedAt { get; set; }

        public string Name { get; set; }

        public int Length { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public byte[] Passport { get; set; }
    }
}
