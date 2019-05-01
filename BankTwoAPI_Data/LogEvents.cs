using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankTwoAPI_Data
{
    public class LogEvents
    {
        //Information
        public static readonly int CreatedSuccessfully = 1000;

        public static readonly int SavedToDatabase = 1100;


        public static readonly int NotFound = 3000;

        //Database Error
        public static readonly int DatabaseError = 5000;

        public static readonly int DatabaseUpdateConcurrencyError = 5100;

        public static readonly int DatabaseNotImplementedError = 5200;


        public static readonly int EmailServerError = 5400;




    }
}
