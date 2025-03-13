using LoanManagementSystem.Models;
using LoanManagementSystem.Repositories;
using LoanManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LoanManagementSystem.Controllers
{
    [Route("api/kyc")]
    [ApiController]
    public class KycController : ControllerBase
    {
        private readonly IKycRepository _kycRepository;
        private readonly IUserRepository _userRepository;
        private readonly OtpService _otpService;

        public KycController(IKycRepository kycRepository, IUserRepository userRepository, OtpService otpService)
        {
            _kycRepository = kycRepository;
            _userRepository = userRepository;
            _otpService = otpService;
        }

        // ✅ Send OTP Before KYC Submission
        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOtp([FromBody] OtpRequest request)
        {
            var user = await _userRepository.GetUserByEmail(request.Email);
            if (user == null) return NotFound(new { message = "User not found." });

            string otp = _otpService.GenerateOtp(request.Email);
            return Ok(new { message = "OTP sent to email." });
        }

        // ✅ Apply for KYC after OTP verification
        [HttpPost("apply")]
        public async Task<IActionResult> ApplyKyc([FromBody] KycRequest request)
        {
            var user = await _userRepository.GetUserById(request.UserId);
            if (user == null) return NotFound(new { message = "User not found." });

            bool isOtpValid = _otpService.VerifyOtp(request.Email, request.Otp);
            if (!isOtpValid) return BadRequest(new { message = "Invalid OTP." });

            Kyc kyc = new Kyc
            {
                UserId = request.UserId,
                Email = request.Email,
                AadharNumber = request.AadharNumber,
                PanNumber = request.PanNumber,
                BankAccountNumber = request.BankAccountNumber,
                FullName = request.FullName,
                DateOfBirth = request.DateOfBirth,
                Status = "Pending",
                AppliedTime = DateTime.UtcNow
            };

            await _kycRepository.AddKyc(kyc);
            await _kycRepository.SaveChanges();

            return Ok(new { message = "KYC application submitted successfully.", kycId = kyc.Id });
        }

        // ✅ Approve KYC
        [HttpPut("approve/{id}")]
        public async Task<IActionResult> ApproveKyc(int id)
        {
            var kyc = await _kycRepository.GetKycById(id);
            if (kyc == null) return NotFound(new { message = "KYC record not found." });

            kyc.Status = "Approved";
            kyc.ApprovedTime = DateTime.UtcNow;
            await _kycRepository.SaveChanges();

            return Ok(new { message = "KYC approved successfully." });
        }

        // ✅ Reject KYC
        [HttpPut("reject/{id}")]
        public async Task<IActionResult> RejectKyc(int id)
        {
            var kyc = await _kycRepository.GetKycById(id);
            if (kyc == null) return NotFound(new { message = "KYC record not found." });

            kyc.Status = "Rejected";
            kyc.RejectedTime = DateTime.UtcNow;
            await _kycRepository.SaveChanges();

            return Ok(new { message = "KYC rejected successfully." });
        }

        // ✅ Get All KYC Records
        [HttpGet("all")]
        public async Task<IActionResult> GetAllKycs()
        {
            var kycs = await _kycRepository.GetAllKycs();
            return Ok(kycs);
        }

        // ✅ Get KYC by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetKycById(int id)
        {
            var kyc = await _kycRepository.GetKycById(id);
            if (kyc == null) return NotFound(new { message = "KYC record not found." });
            return Ok(kyc);
        }

        // ✅ Get KYC by User ID
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetKycByUserId(int userId)
        {
            var kyc = await _kycRepository.GetKycByUserId(userId);
            if (kyc == null) return NotFound(new { message = "No KYC record found for this user." });
            return Ok(kyc);
        }
    }

    public class OtpRequest
    {
        public string Email { get; set; }
    }

    public class KycRequest
    {
        public int UserId { get; set; }
        public string Otp { get; set; }
        public string Email { get; set; }
        public string AadharNumber { get; set; }
        public string PanNumber { get; set; }
        public string BankAccountNumber { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}