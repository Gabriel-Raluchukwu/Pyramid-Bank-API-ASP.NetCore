using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankTwoCoreAPI.UtilityLogic
{
    public class PasswordHash
    {
        public enum Authentication
        {
            failure,
            success
        }
        //TODO: Implement Pasword Hash function
        //NOTE: Google best hashing in C# for password. Include customerAccount Id
       

        public static string HashPassword(string password,string salt)
        {
            string Password;
            byte[] hashedPassword;
          
            using (var hmac = new System.Security.Cryptography.HMACSHA512(Encoding.UTF8.GetBytes(salt)))
            {
                hashedPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }

            //
            StringBuilder passwordStringBuilder = new StringBuilder();
            foreach (var letter in hashedPassword)
            {
                passwordStringBuilder.Append(letter.ToString("x2"));
            }
            Password = passwordStringBuilder.ToString();;
            return Password;
        }

        public static Authentication VerifyPassword(string password,string storedPassword, string storedSalt)
        {
            string confirmPassword;
            byte[] passwordHash;
            using (var hmac = new System.Security.Cryptography.HMACSHA512(Encoding.UTF8.GetBytes(storedSalt)))
            {
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
            StringBuilder passwordStringBuilder = new StringBuilder();
            foreach (var letter in passwordHash)
            {
                passwordStringBuilder.Append(letter.ToString("x2"));
            }
            confirmPassword = passwordStringBuilder.ToString();
            if (confirmPassword.Length == storedPassword.Length)
            {
                if (confirmPassword == storedPassword)
                {
                    return Authentication.success;
                }
       
            }
            return Authentication.failure;
        }
    }
}
