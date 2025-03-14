using LoanManagementSystem.Data;
using LoanManagementSystem.Models;
using LoanManagementSystem.Repositories;
using LoanManagementSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
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
        private readonly LoanDbContext _context;

        public LoanController(
            ILoanRepository loanRepository,
            IKycRepository kycRepository,
            ICreditCardRepository creditCardRepository,
            EmailService emailService,
            LoanDbContext context)
        {
            _loanRepository = loanRepository;
            _kycRepository = kycRepository;
            _creditCardRepository = creditCardRepository;
            _emailService = emailService;
            _context = context;
        }

        // ✅ Get All Loan Applications (New Endpoint)
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllLoans()
        {
            var loans = await _loanRepository.GetAllLoans();
            var loanDtos = loans.Select(l => MapToLoanDto(l)).ToList();
            return Ok(loanDtos);
        }

        // ✅ Apply for a Loan
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

            var existingLoan = await _loanRepository.GetLoanByUserId(request.UserId);
            if (existingLoan != null && existingLoan.Status == "Pending")
            {
                return BadRequest("You already have a pending loan.");
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
        [HttpPut("approve/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
                UserId = loan.UserId,
                CardNumber = await GenerateUniqueCardNumberAsync(),
                ExpiryDate = DateTime.UtcNow.AddYears(3),
                CVV = new Random().Next(100, 999).ToString(),
                ApprovedAmount = loan.Amount / 2
            };

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    await _creditCardRepository.AddCreditCard(digitalCard);
                    await _loanRepository.SaveChanges();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new { message = "An error occurred while approving the loan.", details = ex.Message });
                }
            }

            return Ok(new { message = "Loan approved successfully, and Digital Credit Card generated." });
        }

        // ✅ Onboard User After Loan Approval
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

        // ✅ Generate Unique Card Number
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

        // ✅ Mapping Method for Loan DTO
      // Inside LoanController.cs
private LoanResponseDto MapToLoanDto(Loan loan)
{
    return new LoanResponseDto
    {
        Id = loan.Id,
        UserId = loan.UserId ?? 0, // Default to 0 if null; assumes null is invalid
        UserName = loan.User?.Username ?? "Unknown",
        Amount = loan.Amount,
        Purpose = loan.Purpose,
        MonthlySalary = loan.MonthlySalary,
        Status = loan.Status,
        AppliedTime = loan.AppliedTime,
        ApprovedTime = loan.ApprovedTime,
        RejectedTime = loan.RejectedTime
    };
}
    }
}