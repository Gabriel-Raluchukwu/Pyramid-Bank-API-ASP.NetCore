using System;
using System.Collections.Generic;
using System.Text;

namespace BankTwoAPI_Entities.Models
{
    public class BaseEntity
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsActive { get; set; }

        public BaseEntity()
        {
            IsActive = true;
        }
    }
}
