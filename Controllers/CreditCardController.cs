using LoanManagementSystem.Models;
using LoanManagementSystem.Repositories;
using LoanManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LoanManagementSystem.Controllers
{
    [Route("api/credit-cards")]
    [ApiController]
    public class CreditCardController : ControllerBase
    {
        private readonly ICreditCardRepository _creditCardRepository;
        private readonly EmailService _emailService;

        public CreditCardController(ICreditCardRepository creditCardRepository, EmailService emailService)
        {
            _creditCardRepository = creditCardRepository;
            _emailService = emailService;
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> TransferCreditAmount([FromBody] CreditTransferRequest request)
        {
            var senderCard = await _creditCardRepository.GetCreditCardByNumberAsync(request.SenderCardNumber);
            var receiverCard = await _creditCardRepository.GetCreditCardByNumberAsync(request.ReceiverCardNumber);

            if (senderCard == null || receiverCard == null)
            {
                return BadRequest("Invalid sender or receiver credit card number.");
            }

            if (senderCard.ApprovedAmount < request.Amount)
            {
                return BadRequest("Insufficient funds in the sender's credit card.");
            }

            // Since .Include(c => c.User) is in the repository, User should not be null
            // If it’s still null, it’s a data issue (e.g., invalid UserId)
            if (senderCard.User == null || receiverCard.User == null)
            {
                return StatusCode(500, $"User data missing: Sender UserId={senderCard.UserId}, Receiver UserId={receiverCard.UserId}");
            }

            senderCard.ApprovedAmount -= request.Amount;
            receiverCard.ApprovedAmount += request.Amount;

            var transaction = new CreditCardTransaction
            {
                SenderCardNumber = senderCard.CardNumber,
                ReceiverCardNumber = receiverCard.CardNumber,
                Amount = request.Amount,
                TransactionDate = DateTime.UtcNow
            };

            await _creditCardRepository.AddTransaction(transaction);
            await _creditCardRepository.SaveChanges();

            await _emailService.SendEmailAsync(senderCard.User.Email,
                "💸 Debit Alert",
                $"₹{request.Amount} has been debited from your card. Remaining balance: ₹{senderCard.ApprovedAmount}");

            await _emailService.SendEmailAsync(receiverCard.User.Email,
                "💰 Credit Alert",
                $"₹{request.Amount} has been credited to your card from {senderCard.User.Username}.");

            return Ok(new { message = "Transfer successful." });
        }

        [HttpGet("transactions/{cardNumber}")]
        public async Task<IActionResult> GetTransactionHistory(string cardNumber)
        {
            var transactions = await _creditCardRepository.GetTransactionsByCardNumber(cardNumber);
            if (transactions == null || transactions.Count == 0)
            {
                return NotFound("No transaction history found.");
            }

            return Ok(transactions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCreditCardById(int id)
        {
            var creditCard = await _creditCardRepository.GetCreditCardById(id);
            if (creditCard == null) return NotFound("Credit card not found.");
            return Ok(creditCard);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCreditCard(int id)
        {
            var creditCard = await _creditCardRepository.GetCreditCardById(id);
            if (creditCard == null) return NotFound("Credit card not found.");

            _creditCardRepository.DeleteCreditCard(creditCard);
            await _creditCardRepository.SaveChanges();

            return Ok("Credit card deleted successfully.");
        }
    }

    public class CreditTransferRequest
    {
        public string SenderCardNumber { get; set; }
        public string ReceiverCardNumber { get; set; }
        public decimal Amount { get; set; }
    }
}