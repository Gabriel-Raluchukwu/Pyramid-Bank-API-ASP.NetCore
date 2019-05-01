using System;
using System.Collections.Generic;
using System.Text;

namespace BankTwoAPI_Entities
{
    public class Validation
    {
        public static bool ValidLength(int TransactionPin)
        {
            const int verificationCodeLength = 4;
            string verificationCode = TransactionPin.ToString();
            return (verificationCode.Length == verificationCodeLength) ? true : false;
        }

    }
}
