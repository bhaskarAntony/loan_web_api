using System;
using System.Net;
using System.Net.Mail;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace LoanManagementSystem.Services
{
    public class OtpService
    {
        private static readonly Dictionary<string, string> OtpStore = new Dictionary<string, string>();
        private readonly IConfiguration _configuration;

        public OtpService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateOtp(string email)
        {
            Random rand = new Random();
            string otp = rand.Next(100000, 999999).ToString();
            OtpStore[email] = otp;

            SendEmail(email, otp);
            return otp;
        }

        public bool VerifyOtp(string email, string enteredOtp)
        {
            return OtpStore.ContainsKey(email) && OtpStore[email] == enteredOtp;
        }

        private void SendEmail(string email, string otp)
        {
            try
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
                    Subject = "Your OTP Code",
                    Body = $"Your OTP for KYC verification is: {otp}",
                    IsBodyHtml = false
                };
                mailMessage.To.Add(email);

                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending OTP email: {ex.Message}");
                throw;
            }
        }
    }
}
