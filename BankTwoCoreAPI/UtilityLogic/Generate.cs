using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankTwoCoreAPI.UtilityLogic
{
    public class Generate
    {
        private static Random RandomGenerator = new Random();
        private static int accountNumberLength = 10;
        static readonly int[] numbers = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        public static string GenerateAccountNumber()
        {          
            //TODO:Implement GenerateAccountNumber Method
            Random randomGenerator = new Random();
            string accountNumber = "0";
            for (int i = 0; i < accountNumberLength - 1; i++)
            {
                int random = randomGenerator.Next(0, 9);
                accountNumber += numbers[random];
            }
            return accountNumber;
        }

        public static int GenerateVerificationCode()
        {
            return int.Parse(RandomGenerator.Next(0,999999).ToString("D6"));
        }
    }

}
