using LoanManagementSystem.Models;
using LoanManagementSystem.Repositories;
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

        public LoanController(ILoanRepository loanRepository, IKycRepository kycRepository)
        {
            _loanRepository = loanRepository;
            _kycRepository = kycRepository;
        }

        // ✅ Apply for a Loan (Only if KYC is Approved)
        [HttpPost("apply")]
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

        // ✅ Get All Loans
        [HttpGet]
        public async Task<IActionResult> GetAllLoans()
        {
            var loans = await _loanRepository.GetAllLoans();
            return Ok(loans);
        }

        // ✅ Get a Single Loan by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLoanById(int id)
        {
            var loan = await _loanRepository.GetLoanById(id);
            if (loan == null) return NotFound("Loan not found.");
            return Ok(loan);
        }

        // ✅ Update Loan (Only if Pending)
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateLoan(int id, [FromBody] LoanRequest request)
        {
            var loan = await _loanRepository.GetLoanById(id);
            if (loan == null) return NotFound("Loan not found.");
            if (loan.Status != "Pending") return BadRequest("Only pending loans can be updated.");

            loan.Amount = request.Amount;
            loan.Purpose = request.Purpose;
            loan.MonthlySalary = request.MonthlySalary;

            await _loanRepository.SaveChanges();
            return Ok(new { message = "Loan updated successfully." });
        }

        // ✅ Delete Loan (Only if Pending)
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteLoan(int id)
        {
            var loan = await _loanRepository.GetLoanById(id);
            if (loan == null) return NotFound("Loan not found.");
            if (loan.Status != "Pending") return BadRequest("Only pending loans can be deleted.");

            _loanRepository.DeleteLoan(loan);
            await _loanRepository.SaveChanges();
            return Ok(new { message = "Loan deleted successfully." });
        }

        // ✅ Approve Loan
        [HttpPut("approve/{id}")]
        public async Task<IActionResult> ApproveLoan(int id)
        {
            var loan = await _loanRepository.GetLoanById(id);
            if (loan == null) return NotFound("Loan not found.");
            if (loan.Status != "Pending") return BadRequest("Only pending loans can be approved.");

            loan.Status = "Approved";
            loan.ApprovedTime = DateTime.UtcNow;

            await _loanRepository.SaveChanges();
            return Ok(new { message = "Loan approved successfully." });
        }

        // ✅ Reject Loan
        [HttpPut("reject/{id}")]
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
