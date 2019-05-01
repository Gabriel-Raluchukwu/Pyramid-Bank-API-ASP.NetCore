using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Net.Smtp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using BankTwoAPI_Data;

namespace BankTwoCoreAPI.UtilityLogic.Email
{
    public class EmailSender : IEmailSender
    {
        //TODO: Implement Email Sending function
        //NOTE: Send Email to successfully registered customer
        private IConfiguration _configuration { get; }

        private ILogger _logger { get; }

        public EmailSender(IConfiguration configuration,ILogger<EmailSender> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public bool SendEmail(string recepientName, string recipientEmail,string accountNumber,int verificationCode)
        {
            string name = _configuration["Properties:BankName"];
            string emailAddress = _configuration["EmailConfiguration:EmailAdress"];

            var Message = new MimeMessage();
            Message.From.Add(new MailboxAddress("Mbaebie Raluchukwu",emailAddress));
            Message.To.Add(new MailboxAddress(recepientName,recipientEmail));
            Message.Subject = "Aurora Account Details";

            var builder = new BodyBuilder
            {
                TextBody = (@"
                                Welcome to " + name + " bank," +
                                "Account Number: " + accountNumber + "." +
                                "Verification Code: " + verificationCode + "." +
                                "Thank you for Banking with us" +
                                "Customer service: 01-1234556 "
                                ),
                HtmlBody = (@"
                                Welcome to " + name + " bank,<br>" +
                                "Account Number: " + accountNumber + ".<br>" +
                                "Verification Code: " + verificationCode + ".<br>" +
                                "Thank you for Banking with us<br><br>" +
                                "<b>Customer service: 01-1234556</b> ")
        };

            Message.Body = builder.ToMessageBody();
            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587);

                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate("Mbaebie Raluchukwu", _configuration["EmailConfiguration:Password"]);

                try
                {
                    client.Send(Message);
                    client.Disconnect(true);
                }
                catch (Exception e)
                {
                    _logger.LogError(LogEvents.EmailServerError,e,e.Message,Message);
                    return false;
                }

                return true;
            }
            
        } 
    }
}