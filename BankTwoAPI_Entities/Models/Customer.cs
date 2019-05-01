using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BankTwoAPI_Entities.Models
{
    public class Customer : BaseEntity
    {
        public DateTime LastUpdatedAt { get; set; }

        public string CustomerAccountNumber { get; set; }

        public string UserName { get; set; }

        public string PasswordHash { get; set; }

        public string PasswordSalt { get; set; }

        public int VerificationCode { get; set; }

        public virtual ICollection<AccountCustomerCategory> CustomerAccounts { get; set; }

    }
}
