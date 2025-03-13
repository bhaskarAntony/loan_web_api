using LoanManagementSystem.Models;
using LoanManagementSystem.Repositories;
using LoanManagementSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LoanManagementSystem.Controllers
{
    [Route("api/loans")]
    [ApiController]
    public class LoanController : ControllerBase
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IKycRepository _kycRepository;
        private readonly ICreditCardRepository _creditCardRepository;
        private readonly EmailService _emailService;

        public LoanController(
            ILoanRepository loanRepository,
            IKycRepository kycRepository,
            ICreditCardRepository creditCardRepository,
            EmailService emailService)
        {
            _loanRepository = loanRepository;
            _kycRepository = kycRepository;
            _creditCardRepository = creditCardRepository;
            _emailService = emailService;
        }

        // ✅ Apply for a Loan (Only if KYC is Approved)
        /// <summary>
        /// Apply for a loan with an approved KYC
        /// </summary>
        [HttpPost("apply")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ApplyLoan([FromBody] LoanRequest request)
        {
            var kyc = await _kycRepository.GetKycByUserId(request.UserId);
            if (kyc == null || kyc.Status != "Approved")
            {
                return BadRequest("Loan cannot be applied without an approved KYC.");
            }

            Loan loan = new Loan
            {
                UserId = request.UserId,
                Amount = request.Amount,
                Purpose = request.Purpose,
                MonthlySalary = request.MonthlySalary,
                Status = "Pending",
                AppliedTime = DateTime.UtcNow
            };

            await _loanRepository.AddLoan(loan);
            await _loanRepository.SaveChanges();
            return Ok(new { message = "Loan application submitted successfully." });
        }

        // ✅ Approve Loan and Generate Digital Credit Card
        /// <summary>
        /// Approve loan and generate a digital credit card
        /// </summary>
        [HttpPut("approve/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ApproveLoan(int id)
        {
            var loan = await _loanRepository.GetLoanById(id);
            if (loan == null) return NotFound("Loan not found.");
            if (loan.Status != "Pending") return BadRequest("Only pending loans can be approved.");

            loan.Status = "Approved";
            loan.ApprovedTime = DateTime.UtcNow;

            var digitalCard = new CreditCard
            {
                LoanId = loan.Id,
                CardNumber = await GenerateUniqueCardNumberAsync(),
                ExpiryDate = DateTime.UtcNow.AddYears(3),
                CVV = new Random().Next(100, 999).ToString(),
                ApprovedAmount = loan.Amount / 2
            };

            await _creditCardRepository.AddCreditCard(digitalCard);
            await _creditCardRepository.SaveChanges();

            return Ok(new { message = "Loan approved successfully, and Digital Credit Card generated." });
        }

        // ✅ Onboard User After Loan Approval
        /// <summary>
        /// Onboard a user after loan approval
        /// </summary>
        [HttpPost("onboard/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> OnboardUser(int userId)
        {
            var loan = await _loanRepository.GetLoanByUserId(userId);
            if (loan == null || loan.Status != "Approved")
            {
                return BadRequest("Onboarding cannot proceed without an approved loan.");
            }

            var digitalCard = await _creditCardRepository.GetCreditCardByLoanId(loan.Id);
            if (digitalCard == null)
            {
                return BadRequest("No associated credit card found for the approved loan.");
            }

            string htmlBody = _emailService.GenerateOnboardingEmail(
                userName: loan.User.Username,
                loanAmount: loan.Amount.ToString("N2"),
                creditCardNumber: digitalCard.CardNumber,
                expiryDate: digitalCard.ExpiryDate.ToString("MM/yyyy")
            );

            await _emailService.SendEmailAsync(
                loan.User.Email,
                "🎯 Welcome to Loan Management System!",
                htmlBody
            );

            return Ok(new { message = "Onboarding email sent successfully." });
        }

        // ✅ Reject Loan
        /// <summary>
        /// Reject a loan application
        /// </summary>
        [HttpPut("reject/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RejectLoan(int id)
        {
            var loan = await _loanRepository.GetLoanById(id);
            if (loan == null) return NotFound("Loan not found.");
            if (loan.Status != "Pending") return BadRequest("Only pending loans can be rejected.");

            loan.Status = "Rejected";
            loan.RejectedTime = DateTime.UtcNow;

            await _loanRepository.SaveChanges();
            return Ok(new { message = "Loan rejected successfully." });
        }

        // ✅ Improved Method for Unique 16-digit Card Number Generation
        private async Task<string> GenerateUniqueCardNumberAsync()
        {
            string cardNumber;
            bool isUnique = false;

            do
            {
                cardNumber = "4111" + new Random().Next(100000000, 999999999).ToString();
                var existingCard = await _creditCardRepository.GetCreditCardByNumberAsync(cardNumber);
                if (existingCard == null) isUnique = true;
            } while (!isUnique);

            return cardNumber;
        }
    }

    // ✅ Loan Request DTO
    public class LoanRequest
    {
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public string Purpose { get; set; }
        public decimal MonthlySalary { get; set; }
    }
}
