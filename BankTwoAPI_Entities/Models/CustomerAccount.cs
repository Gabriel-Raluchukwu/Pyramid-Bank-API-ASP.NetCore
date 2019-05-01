using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BankTwoAPI_Entities.Models
{
    public class CustomerAccount:BaseEntity
    {
        public DateTime LastUpdatedAt { get; set; }

        public string CustomerAccountNumber { get; set; }

        public string Surname { get; set; }

        public string FirstName { get; set; }

        public string OtherNames { get; set; }

        public string BVNNumber { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public CustomerAccountType AccountType { get; set; }

        public decimal AccountBalance { get; set; }

        public int TransactionPin { get; set; }

        public virtual ICollection<AccountCustomerCategory> Customers { get; set; }
    }
    public enum CustomerAccountType
    {
        Savings = 1,
        Current
    }

}
