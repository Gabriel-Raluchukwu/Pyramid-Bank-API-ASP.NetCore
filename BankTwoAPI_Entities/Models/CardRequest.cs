using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BankTwoAPI_Entities.Models
{
    public class CardRequest:BaseEntity
    {
        [ForeignKey("CustomerAccount")]
        public int CustomerAccountId { get; set; }

        public CustomerAccount CustomerAccount { get; set; }

        public string CustomerAccountNumber { get; set; }

        public string Description { get; set; }

        public CardType CardType { get; set; }

    }

    public enum CardType
    {
        VerveCard,
        MasterCard,
        //DollarMasterCard
    }
}
