using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace LoanManagementSystem.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string htmlBody)
        {
            string smtpServer = _configuration["Smtp:Server"];
            int smtpPort = int.Parse(_configuration["Smtp:Port"]);
            string smtpUsername = _configuration["Smtp:Username"];
            string smtpPassword = _configuration["Smtp:Password"];

            var smtpClient = new SmtpClient(smtpServer)
            {
                Port = smtpPort,
                Credentials = new NetworkCredential(smtpUsername, smtpPassword),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(smtpUsername),
                Subject = subject,
                Body = htmlBody,
                IsBodyHtml = true
            };
            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);
        }

        public string GenerateOnboardingEmail(string userName, string loanAmount, string creditCardNumber, string expiryDate)
        {
            return $@"
            <html>
            <body>
                <h2>Welcome, {userName}!</h2>
                <p>Your loan of <b>{loanAmount}</b> has been approved.</p>
                <p>Your digital credit card details are:</p>
                <ul>
                    <li><b>Card Number:</b> {creditCardNumber}</li>
                    <li><b>Expiry Date:</b> {expiryDate}</li>
                </ul>
                <p>Thank you for choosing our Loan Management System.</p>
                <hr>
                <p style='font-size: 12px;'>This is an automated email, please do not reply.</p>
            </body>
            </html>";
        }
    }
}
