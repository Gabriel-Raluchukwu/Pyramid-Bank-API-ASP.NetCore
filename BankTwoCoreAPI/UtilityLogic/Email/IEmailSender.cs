using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankTwoCoreAPI.UtilityLogic.Email
{
    public interface IEmailSender
    {
        bool SendEmail(string recepientName, string recipientEmail, string accountNumber, int verificationCode);
    }
}
